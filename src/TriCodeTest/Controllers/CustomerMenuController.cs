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

        public IActionResult PostMenuItemToCart(int id)
        {
            var TEST = id;
            var MenuItem = _context.MenuItem.FirstOrDefault(mi => mi.Id == id);

            return RedirectToAction(nameof(CustomerMenuController.Cart), "CustomerMenu", null);
        }

        public async Task<IActionResult> Cart()
        {
            var user = await GetCurrentUserAsync();
            var cart = await _context.OrderInfo.Where(s => s.Status == Models.Status.Cart).Where(usr => usr.User.Id == user.Id).SingleOrDefaultAsync();
            if (cart != null)
            {
                return View(OrderDeserialize(cart));
            }
            AddNewOrderToDatabase(user);
            Order viewModelOrder = OrderDeserialize(await _context.OrderInfo.Where(s => s.Status == Models.Status.Cart).Where(usr => usr.User.Id == user.Id).SingleOrDefaultAsync());
            return View(cart);
        }

        //public IActionResult LoadCart()
        //{
        //    Get info cart;
        //}

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private void AddNewOrderToDatabase(ApplicationUser user)
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
        }

        private Order OrderDeserialize(OrderInfo model)
        {
            Order OrderModel = new Order
            {
                Id = model.Id,
                User = model.User,
                DateTime = model.DateTime,
                Status = model.Status,
                TotalPrice = model.TotalPrice,
            };
            if (OrderModel.OrderMenuItems != null)
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
    }
}