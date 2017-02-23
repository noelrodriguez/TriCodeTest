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
using Newtonsoft.Json;

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

        // POST: MenuCreation/AddIngredient
        [HttpPost]
        public ActionResult AddIngredient(Ingredient obj)
        {
            return Json(false);
        }

        // POST: MenuCreation/AddCategory
        // Adds the specified category object to the database and returns that updated category
        [HttpPost]
        public async Task<ActionResult> AddCategory(Category obj)
        {
            _context.Add(obj); //Add to the database
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if(updated < 1) //determine that at least one item was added to the database
            {
                return NotFound();
            }

            return Json(obj); //Return the updated object back to view after it has been added to the database.
        }

        // POST: MenuCreation/RemoveCategory
        //Removes category specified by id
        [HttpPost]
        public async Task<ActionResult> RemoveCategory(int id)
        {
            System.Diagnostics.Debug.WriteLine(id);
            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        // POST: MenuCreation/EditCategory
        // given category object update this category

        [HttpPost]
        public async Task<ActionResult> EditCategory(Category obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(obj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Json(true);
        }

        // POST: MenuCreation/AddSubcategory
        // Given subcategory, category, and a list of addons update the database and return the
        // subcategory object.

        [HttpPost]
        public async Task<ActionResult> AddSubcategory(Subcategory subcategoryObj, Category categoryObj, List<AddOn> addonsObj)
        {
            Subcategory newSubcategory = new Subcategory()
            {
                Id = subcategoryObj.Id,
                Name = subcategoryObj.Name,
                Description = subcategoryObj.Description,
                CategoryId = categoryObj.Id
            };


            _context.Subcategory.Add(newSubcategory);
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if (updated < 1) //determine that at least one item was added to the database
            {
                return NotFound();
            }

            foreach (var item in addonsObj)
            {
                System.Diagnostics.Debug.WriteLine(item.Name);
                item.SubcategoryId = newSubcategory.Id;
                _context.AddOn.Add(item); //Add each addon to the database
            }
            await _context.SaveChangesAsync();

            var returnObject = await _context.Subcategory.Include(m => m.Category).Include(w => w.AddOns).FirstOrDefaultAsync(x => x.Id == newSubcategory.Id);

            return Json(returnObject);

        }

        // GET: MenuCreation/GetAddons
        //[HttpGet]
        //public async Task<ActionResult> GetAddons(int id)
        //{
        //    //var Addons = await _context.AddOn.
        //}

        // POST: MenuCreation/RemoveSubcategory
        // Remove the indicated subcategory and return true when complete
        [HttpPost]
        public async Task<ActionResult> RemoveSubcategory(int id)
        {
            System.Diagnostics.Debug.WriteLine(id);
            var subcategory = await _context.Subcategory.SingleOrDefaultAsync(m => m.Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }
            _context.Subcategory.Remove(subcategory);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
