using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
using TriCodeTest.Models.CustomerMenuViewModels;
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
    }
}