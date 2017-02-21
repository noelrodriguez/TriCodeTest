using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Models.MenuViewModels;

namespace TriCodeTest.Controllers
{
    public class MenuCreationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuCreationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuCreation/menu
        public ActionResult Menu()
        {
            MenuCreationViewModel MCVM = new MenuCreationViewModel()
            {
                CategoryMenu = _context.Category.ToList(),
                SubCategoryMenu = _context.Subcategory.ToList(),
                MenuItemMenu = _context.MenuItem.ToList(),
                MenuItemIngredientsMenu = _context.MenuItemIngredients.ToList(),
                AddonMenu = _context.AddOn.ToList(),
                IngridientMenu = _context.Ingredient.ToList()
            };


            //await _context.Category.ToListAsync();

            return View(MCVM);
        }


        // GET: MenuCreation
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
