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
            var user = await GetCurrentUserAsync();
            var menuItem = await _context.MenuItem.Include(ing => ing.MenuItemIngredients).ThenInclude(ing => ing.Ingredient).SingleOrDefaultAsync(mi => mi.Id == id);
            OrderInfo cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();

            if (cart == null)
            {
                cart = AddNewOrderToDatabase(user);
            }
            // Adds menu item to the Order(Cart)
            Order viewModelCart = OrderDeserialize(cart);
            viewModelCart.OrderMenuItems.Add(ConvertToOrderMenuItem(menuItem));
            cart.OrderMenuItems = OrderSerialize(viewModelCart).OrderMenuItems;
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

            var user = await GetCurrentUserAsync();
            var listOfMenuitems = OrderDeserialize(await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync()).OrderMenuItems;
            foreach (var menuitem in listOfMenuitems)
            {
                if (menuitem.MenuItem.Id == id)
                {
                    return View(menuitem);
                }
            }
                
            return RedirectToAction(nameof(CustomerMenuController.Cart), "CustomerMenu", null);
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

        private Order OrderDeserialize(OrderInfo model)
        {
            var test = model;
            Order OrderModel = new Order
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
                OrderMenuItems = new List<OrderMenuItem>()
            };
            if (model.OrderMenuItems != null)
            {
                List<OrderMenuItem> OrderMenuItems = JsonConvert.DeserializeObject<List<OrderMenuItem>>(model.OrderMenuItems);
                OrderModel.OrderMenuItems = OrderMenuItems;
            }
            return OrderModel;
        }

        private OrderInfo OrderSerialize(Order model)
        {
            OrderInfo OrderInfoModel = new OrderInfo
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
            };
            string json = JsonConvert.SerializeObject(model.OrderMenuItems);
            OrderInfoModel.OrderMenuItems = json;
            return OrderInfoModel;
        }

        private OrderMenuItem ConvertToOrderMenuItem(MenuItem item)
        {
            OrderMenuItem orderMenuItem = new OrderMenuItem()
            {
                MenuItem = item,
            };
            return orderMenuItem;
        }
    }
}