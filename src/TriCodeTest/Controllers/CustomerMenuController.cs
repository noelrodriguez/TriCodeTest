using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
using TriCodeTest.Models;
using TriCodeTest.Models.CustomerMenuViewModels;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Models.OrderModels;
using Newtonsoft.Json;

namespace TriCodeTest.Controllers
{
    [Authorize]
    public class CustomerMenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerMenuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CustomerMenu
        public IActionResult Index()
        {
            LoadMenuViewModel LoadMenuViewModel = new LoadMenuViewModel()
            {
                Categories = _context.Category.ToList(),
                Subcategories = _context.Subcategory.ToList(),
                MenuItems = _context.MenuItem.Include(mi => mi.MenuItemIngredients).ThenInclude(mi => mi.Ingredient).ToList()
            };
            return View(LoadMenuViewModel);
        }

        // GET: CustomerMenu/Cart
        public async Task<IActionResult> Cart()
        {
            var user = await GetCurrentUserAsync();
            var cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();
            if (cart != null)
            {
                return View(OrderDeserialize(cart));
            }
            return View(null);
        }

        // POST: CustomerMenu/PostMenuItemToCart/5
        public async Task<IActionResult> PostMenuItemToCart(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            MenuItem menuItem = await _context.MenuItem.Include(ing => ing.MenuItemIngredients).ThenInclude(ing => ing.Ingredient).SingleOrDefaultAsync(mi => mi.Id == id);
            OrderInfo cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();
            Subcategory subcategory = await _context.Subcategory.Include(a => a.AddOns).SingleOrDefaultAsync(s => s.Id == menuItem.SubcategoryId);

            if (cart == null)
            {
                cart = AddNewOrderToDatabase(user);
            }
            // Adds menu item to the Order(Cart)
            CartOrder viewModelCart = OrderDeserialize(cart);
            viewModelCart.CartOrderMenuItems.Add(ConvertToOrderMenuItem(menuItem, subcategory));
            cart.OrderMenuItems = OrderSerialize(viewModelCart).OrderMenuItems;
            cart.TotalPrice += menuItem.Price;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CustomerMenuController.Cart), "CustomerMenu", null);
        }

        // GET: CustomerMenu/EditItem/5
        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await GetCurrentUserAsync();
            List<CartOrderMenuItem> listOfMenuitems = OrderDeserialize(await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync()).CartOrderMenuItems;
            foreach (var menuitem in listOfMenuitems)
            {
                if (menuitem.MenuItem.Id == id)
                {
                    menuitem.AddOns = new List<AddOn>();
                    return View(menuitem);
                }
            }
                
            return RedirectToAction(nameof(CustomerMenuController.Cart), "CustomerMenu", null);
        }

        // POST: CustomerMenu/EditItem/5
        [HttpPost]
        public async Task<ActionResult> EditItem(CartOrderMenuItem item)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            OrderInfo cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();
            CartOrder viewModelCart = OrderDeserialize(cart);

            if (ModelState.IsValid)
            {
                //CartOrderMenuItem menuitem = viewModelCart.CartOrderMenuItems.Find(i => i.MenuItem.Id == item.MenuItem.Id);
                //menuitem.MenuItem.MenuItemIngredients = item.MenuItem.MenuItemIngredients;
                foreach (var menuitem in viewModelCart.CartOrderMenuItems)
                {
                    if (menuitem.MenuItem.Id == item.MenuItem.Id)
                    {
                        menuitem.MenuItem.MenuItemIngredients = item.MenuItem.MenuItemIngredients;
                        menuitem.AddOns = item.AddOns;
                        if (item.AddOns != null)
                        {
                            foreach (AddOn addon in menuitem.AddOns)
                            {
                                viewModelCart.TotalPrice += addon.Price;
                            }
                        }
                    }
                }
                OrderInfo temp = OrderSerialize(viewModelCart);
                cart.OrderMenuItems = temp.OrderMenuItems;
                cart.TotalPrice = temp.TotalPrice;
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private OrderInfo AddNewOrderToDatabase(ApplicationUser user)
        {
            OrderInfo newOrder = new OrderInfo()
            {
                User = user,
                TotalPrice = 0,
                DateTime = System.DateTime.Now,
                Status = Status.Cart
            };
            _context.OrderInfo.Add(newOrder);
            _context.SaveChanges();
            return newOrder;
        }

        private CartOrder OrderDeserialize(OrderInfo model)
        {
            var test = model;
            CartOrder CartOrderModel = new CartOrder
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
                CartOrderMenuItems = new List<CartOrderMenuItem>()
            };
            if (model.OrderMenuItems != null)
            {
                List<CartOrderMenuItem> CartOrderMenuItems = JsonConvert.DeserializeObject<List<CartOrderMenuItem>>(model.OrderMenuItems);
                CartOrderModel.CartOrderMenuItems = CartOrderMenuItems;
            }
            return CartOrderModel;
        }

        private OrderInfo OrderSerialize(CartOrder model)
        {
            OrderInfo OrderInfoModel = new OrderInfo
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
            };
            string json = JsonConvert.SerializeObject(model.CartOrderMenuItems);
            OrderInfoModel.OrderMenuItems = json;
            return OrderInfoModel;
        }

        private CartOrderMenuItem ConvertToOrderMenuItem(MenuItem item, Subcategory subcategory)
        {
            CartOrderMenuItem cartOrderMenuItem = new CartOrderMenuItem()
            {
                MenuItem = item,
                ChoiceOfAddons = subcategory.AddOns
            };
            //if (subcategory.AddOns != null)
            //{
            //    foreach (var addon in subcategory.AddOns)
            //    {
            //        cartOrderMenuItem.ChoiceOfAddons.Add(addon);
            //    }
            //}
            return cartOrderMenuItem;
        }
    }
}