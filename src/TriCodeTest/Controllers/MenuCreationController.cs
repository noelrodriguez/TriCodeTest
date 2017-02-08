using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriCodeTest.Data;
using Microsoft.AspNetCore.Authorization;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Models.MenuViewModels;

namespace TriCodeTest.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // GET: MenuCreation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: MenuCreation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuCreation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        // GET: MenuCreation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: MenuCreation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: MenuCreation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: MenuCreation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
