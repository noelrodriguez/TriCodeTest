using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
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

        public CustomerMenuController(ApplicationDbContext context)
        {
            _context = context;
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
            return RedirectToAction(nameof(CustomerMenuController.Index), "CustomerMenu", null);
        }

        public IActionResult Cart()
        {
            Order singleOrder = new Order();
            var user = await GetCurrentUser();
            var order = _context.OrderInfo.Include(oi => oi.User).Where(s => s.Status == Models.Status.Cart).Where(usr => usr.User == ).ToList();
            return View(order);
        }

        //public IActionResult LoadCart()
        //{
        //    Get info cart;
        //}
    }
}