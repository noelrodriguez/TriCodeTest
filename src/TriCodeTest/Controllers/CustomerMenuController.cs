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
using TriCodeTest.TwilioNotification;

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

        /// <summary>
        /// Receives all items needed to build the menu from the database and returns them as an object with lists
        /// </summary>
        /// <returns>View containing list of categories, subcategories, and menu items</returns>
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

        /// <summary>
        /// Returns a view with the order of the current logged in user
        /// </summary>
        /// <returns>View to show items in cart</returns>
        // GET: CustomerMenu/Cart
        public async Task<IActionResult> Cart()
        {
            var user = await GetCurrentUserAsync();
            var cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id).Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();
            if (cart != null)
            {
                return View(OrderDeserialize(cart));
            }
            cart = AddNewOrderToDatabase(user);
            return View(OrderDeserialize(cart));
        }

        /// <summary>
        /// Adds single menu item to cart
        /// </summary>
        /// <param name="id">Id of the item being added</param>
        /// <returns>Redirects user to cart</returns>
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

        /// <summary>
        /// Returns view containing item passed in to be able to edit it
        /// </summary>
        /// <param name="id">Id of item to edit</param>
        /// <returns>View to edit single item</returns>
        // GET: CustomerMenu/EditItem/5
        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await GetCurrentUserAsync();
            List<CartOrderMenuItem> listOfMenuitems = OrderDeserialize(await _context.OrderInfo.Where(usr => usr.User.Id == user.Id)
                .Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync()).CartOrderMenuItems;
            foreach (var menuitem in listOfMenuitems)
            {
                if (menuitem.MenuItem.Id == id)
                {
                    if (menuitem.AddOns == null)
                    {
                        menuitem.AddOns = new List<AddOn>();
                    }
                    return View(menuitem);
                }
            }
                
            return RedirectToAction(nameof(CustomerMenuController.Cart), "CustomerMenu", null);
        }

        /// <summary>
        /// Updates the edited item in the database
        /// </summary>
        /// <param name="item">Entire item with changes in addons or ingredients</param>
        /// <returns>True or false if item was successfully updated</returns>
        // POST: CustomerMenu/EditItem/5
        [HttpPost]
        public async Task<ActionResult> EditItem(CartOrderMenuItem item)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            OrderInfo cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id)
                .Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();
            CartOrder viewModelCart = OrderDeserialize(cart);

            if (ModelState.IsValid)
            {
                //CartOrderMenuItem menuitem = viewModelCart.CartOrderMenuItems.Find(i => i.MenuItem.Id == item.MenuItem.Id);
                //menuitem.MenuItem.MenuItemIngredients = item.MenuItem.MenuItemIngredients;
                viewModelCart.TotalPrice = 0;
                foreach (var menuitem in viewModelCart.CartOrderMenuItems)
                {
                    if (menuitem.MenuItem.Id == item.MenuItem.Id)
                    {
                        menuitem.MenuItem.MenuItemIngredients = item.MenuItem.MenuItemIngredients;
                        menuitem.AddOns = item.AddOns;
                        menuitem.MenuItem.Size = item.MenuItem.Size;
                    }
                    // Calculate total price
                    viewModelCart.TotalPrice += menuitem.MenuItem.Price;
                    if (menuitem.AddOns != null)
                    {
                        foreach (var addon in menuitem.AddOns)
                        {
                            viewModelCart.TotalPrice += addon.Price;
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

        /// <summary>
        /// Removes passed item from user's cart
        /// </summary>
        /// <param name="item">Item to remove from cart</param>
        /// <returns>True or false if item was successfully removed from cart</returns>
        [HttpPost]
        public async Task<ActionResult> RemoveItem(CartOrderMenuItem item)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            OrderInfo cart = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id)
                .Where(s => s.Status == Models.Status.Cart).SingleOrDefaultAsync();
            CartOrder viewModelCart = OrderDeserialize(cart);

            if (ModelState.IsValid)
            {
                foreach (var menuitem in viewModelCart.CartOrderMenuItems)
                {
                    if (menuitem.MenuItem.Id == item.MenuItem.Id)
                    {
                        viewModelCart.CartOrderMenuItems.Remove(menuitem);
                        break;
                    }
                }
                OrderInfo temp = OrderSerialize(viewModelCart);
                cart.OrderMenuItems = temp.OrderMenuItems;
                cart.TotalPrice = CalculateTotalPrice(viewModelCart);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        /// <summary>
        /// Submits the order and changes it's status from Cart to Received
        /// </summary>
        /// <param name="order">Order containing menu items</param>
        /// <returns>True or false if successfully submitted order</returns>
        // POST: CustomerMenu/SubmitOrder/{OrderObject}
        [HttpPost]
        public async Task<ActionResult> SubmitOrder(CartOrder order)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            OrderInfo orderFromDB = await _context.OrderInfo.Where(usr => usr.User.Id == user.Id)
                .Where(o => o.Id == order.Id).SingleOrDefaultAsync();
            if (ModelState.IsValid)
            {
                orderFromDB.Status = Status.Received;
                orderFromDB.DateTime = System.DateTime.Now;
                await _context.SaveChangesAsync();
            }
            try
            {
                bool success = Notification.SendNotification(user.PhoneNumber, "Your order has been received."
                                            + " Estimated time: 15mins depending on the queue. DO NOT REPLY! Data rates may apply");
                if (success)
                {
                    return Json(true);
                }
            }
            catch (Exception e)
            {
                return Json(false);
            }
            return Json(false);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        /// <summary>
        /// Creates a new empty cart if a cart hasn't been created already
        /// </summary>
        /// <param name="user">User to create the cart for</param>
        /// <returns>Newly created order</returns>
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

        /// <summary>
        /// Deserializes an Order and converts it to the view model
        /// </summary>
        /// <param name="model">Order from the databsase</param>
        /// <returns>View model of order with deserialized JSON for items in order</returns>
        private CartOrder OrderDeserialize(OrderInfo model)
        {
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

        /// <summary>
        /// Serializes a view model and converts it to an Order
        /// </summary>
        /// <param name="model">Order view model</param>
        /// <returns>Table model from the database with serialized JSON</returns>
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

        /// <summary>
        /// Converts item to view model containing choice of addons tha can be added
        /// </summary>
        /// <param name="item">Menu item to convert to view model</param>
        /// <param name="subcategory">Subcategory to grab addons from</param>
        /// <returns>View model of item and addons</returns>
        private CartOrderMenuItem ConvertToOrderMenuItem(MenuItem item, Subcategory subcategory)
        {
            CartOrderMenuItem cartOrderMenuItem = new CartOrderMenuItem()
            {
                MenuItem = item,
                ChoiceOfAddons = subcategory.AddOns
            };
            return cartOrderMenuItem;
        }

        /// <summary>
        /// Calculates total price of order
        /// </summary>
        /// <param name="order">Order to calculate price for</param>
        /// <returns>Total price</returns>
        private double CalculateTotalPrice(CartOrder order)
        {
            double totalPrice = 0;
            foreach (var item in order.CartOrderMenuItems)
            {
                totalPrice += item.MenuItem.Price;
                if (item.AddOns != null)
                {
                    foreach (var addon in item.AddOns)
                    {
                        totalPrice += addon.Price;
                    }
                }
            }
            return totalPrice;
        }
    }
}