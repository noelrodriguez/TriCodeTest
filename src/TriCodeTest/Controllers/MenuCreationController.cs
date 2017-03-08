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
    /// <summary>
    /// MenuCreationController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class MenuCreationController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuCreationController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MenuCreationController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Main menu creation page generation. Takes all menu data from the Menu tables
        /// then uses the MenuCreationViewModel to cast that data to a list so the menu creation
        /// page can use it to populate the page with menu data.
        /// </summary>
        /// <returns>
        /// MenuCreationViewModel
        /// </returns>
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

        /// <summary>
        /// Index listener for MenuCreationController. This is antiquated at this point.
        /// </summary>
        /// <returns>
        /// A category list to the view. This is antiquated at this point
        /// </returns>
        // GET: MenuCreation
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // POST: MenuCreation/AddCategory
        // Adds the specified category object to the database and returns that updated category
        /// <summary>
        /// Adds the specified category object to the database and returns that updated category
        /// </summary>
        /// <param name="obj">Category Model Object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddCategory(Category obj)
        {
            _context.Add(obj); //Add to the database
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if (updated < 1) //determine that at least one item was added to the database
            {
                return NotFound();
            }

            return Json(obj); //Return the updated object back to view after it has been added to the database.
        }

        // POST: MenuCreation/RemoveCategory
        //Removes category specified by id
        /// <summary>
        /// Removes a Category specified by its Id.
        /// </summary>
        /// <param name="id">Id of the category which is to be removed</param>
        /// <returns>
        /// True once category has been removed.
        /// </returns>
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
        /// <summary>
        /// Edits a given Category with new data specified by the parameters.
        /// </summary>
        /// <param name="obj">Category Model Object containing data to update existing category.</param>
        /// <returns>
        /// True when update has completed.
        /// </returns>
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

        /// <summary>
        /// Get request for a specific subcategory with a specific id
        /// </summary>
        /// <param name="id">Database Id of a subcategory</param>
        /// <returns>
        /// Returns a subcategory model object matching the id
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> GetSubcategory(int id)
        {

            var subcategory = await _context.Subcategory.SingleOrDefaultAsync(s => s.Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return Json(subcategory);
        }

        // POST: MenuCreation/AddSubcategory
        // Given subcategory, category, and a list of addons update the database and return the
        // subcategory object.
        /// <summary>
        /// Add a new subcategory to the database
        /// </summary>
        /// <param name="subcategoryObj">Subcategory Model Object containing data to add</param>
        /// <param name="categoryObj">Category Model Object which should be tied to the new subcategory</param>
        /// <param name="addonsObj">Addon Model List which contains a list of addons contained in the new subcategory</param>
        /// <returns>
        /// True when completed.
        /// </returns>
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

            return Json(newSubcategory);

        }
        /// <summary>
        /// Given a subcategory model object update the existing subcategory in the database with the new data
        /// </summary>
        /// <param name="obj">Subcategory Model Object</param>
        /// <returns>
        /// Returns true when object is updated in database
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> EditSubcategory(Subcategory obj)
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
                    if (!_context.Subcategory.Any(e => e.Id == obj.Id))
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
        /// <summary>
        /// Returns a list of the addons with the specific subcategory id.
        /// </summary>
        /// <param name="id">The id of the subcategory containing the addons.</param>
        /// <returns>
        /// List of addons
        /// </returns>
        // GET: MenuCreation/GetAddons
        //Given subcategory construct list of addons and return them!
        [HttpGet]
        public ActionResult GetAddons(int id)
        {
            List<AddOn> addons = new List<AddOn>();
            foreach (var addon in _context.AddOn)
            {
                if (addon.SubcategoryId == id)
                {
                    addons.Add(addon);
                }
            }
            return Json(addons);
        }
        /// <summary>
        /// Takes an Addon Model Object and adds it to the database
        /// </summary>
        /// <param name="obj">Addon Model Object</param>
        /// <returns>
        /// Returns addon model object when complete
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> AddAddon(AddOn obj)
        {
            _context.AddOn.Add(obj); //Add to the database
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if (updated < 1) //determine that at least one item was added to the database
            {
                return NotFound();
            }

            return Json(obj); //Return the updated object back to view after it has been added to the database.
        }
        /// <summary>
        /// Given an addon id search the database and remove the addon with that id.
        /// </summary>
        /// <param name="id">database id of addon</param>
        /// <returns>Returns true when remove is complete</returns>
        [HttpPost]
        public async Task<ActionResult> RemoveAddon(int id)
        {
            var addon = await _context.AddOn.SingleOrDefaultAsync(a => a.Id == id);
            if (addon == null)
            {
                return NotFound();
            }
            _context.AddOn.Remove(addon);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        /// <summary>
        /// Removes a subcategory from the database given that subcategories Id.
        /// Returns true when completed.
        /// </summary>
        /// <param name="id">The id of the subcategory to remove.</param>
        /// <returns>
        /// True when function succeeds.
        /// </returns>
        // POST: MenuCreation/RemoveSubcategory
        // Remove the indicated subcategory and return true when complete
        [HttpPost]
        public async Task<ActionResult> RemoveSubcategory(int id)
        {
            var subcategory = await _context.Subcategory.SingleOrDefaultAsync(m => m.Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }
            _context.Subcategory.Remove(subcategory);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        /// <summary>
        /// Add a new menu item to the database and return that updated object
        /// </summary>
        /// <param name="obj">MenuItem Model</param>
        /// <returns>
        /// A MenuItemModel object back to the poster.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> AddMenuItem(MenuItem obj)
        {
            MenuItem newItem = new MenuItem()
            {
                Name = obj.Name,
                Description = obj.Description,
                Price = obj.Price,
                SubcategoryId = obj.SubcategoryId
            };


            _context.MenuItem.Add(newItem); //Add to the database
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if (updated < 1) //determine that at least one item was added to the database
            {
                return NotFound();
            }

            return Json(newItem);
        }
        /// <summary>
        /// Update a menu item with a new image. Takes the id of the menu item to update,
        /// and a base64 image string and converts that string to a byte array to store in
        /// in the database.
        /// </summary>
        /// <param name="id">Id of the menu item to update</param>
        /// <param name="img">A base64 image string</param>
        /// <returns>
        /// Returns true when update is complete
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> UpdateItemImage(int id, String img)
        {
            var menuitem = await _context.MenuItem.SingleOrDefaultAsync(m => m.Id == id);
            if (menuitem == null)
            {
                return NotFound();
            }
            //Convert base64 image string to byte array and add to the database.
            byte[] image;
            image = Convert.FromBase64String(img);
            menuitem.ItemImage = image;
            _context.MenuItem.Update(menuitem);
            var updated = await _context.SaveChangesAsync();
            if (updated < 1)
            {
                return NotFound();
            }

            return Json(true);
        }
        /// <summary>
        /// Given a menu item id retrieves a menu item and returns
        /// the menu item image.
        /// </summary>
        /// <param name="id">Menuitem Id</param>
        /// <returns>
        /// Base64 image string
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> GetItemImage(int id)
        {
            var menuitem = await _context.MenuItem.SingleOrDefaultAsync(m => m.Id == id);
            if (menuitem == null)
            {
                return NotFound();
            }
            //Convert byte arrray to base64 string
            string image = Convert.ToBase64String(menuitem.ItemImage);

            return Json(image);
        }
        /// <summary>
        /// Edit a specific menu item with new data in the database.
        /// </summary>
        /// <param name="obj">New menu item data to update with</param>
        /// <returns>
        /// Returns true when update is complete
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> EditMenuItem(MenuItem obj)
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
                    if (!_context.MenuItem.Any(e => e.Id == obj.Id))
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

        /// <summary>
        /// Remove a menuitem from the database with a specific id
        /// </summary>
        /// <param name="id">The id of the Menuitem to remove.</param>
        /// <returns>
        /// Returns true when removal succeeds!
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> RemoveMenuItem(int id)
        {
            var menuitem = await _context.MenuItem.SingleOrDefaultAsync(m => m.Id == id);
            if (menuitem == null)
            {
                return NotFound();
            }
            _context.MenuItem.Remove(menuitem);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        /// <summary>
        /// Retrieve a menu item from the database with a specific id
        /// </summary>
        /// <param name="id">Menu item id to retrieve</param>
        /// <returns>
        /// Menuitem object
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> GetMenuItem(int id)
        {
            var menuitem = await _context.MenuItem.SingleOrDefaultAsync(s => s.Id == id);
            if (menuitem == null)
            {
                return NotFound();
            }

            return Json(menuitem);
        }
        /// <summary>
        /// Add a new Ingredient to the database and return that new ingredient
        /// </summary>
        /// <param name="obj">Ingredient Model</param>
        /// <returns>
        /// A new ingredient model with updated data
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> AddIngredient(Ingredient obj)
        {
            

            //enum ingredientOption { None, Low, Normal };
            Ingredient newItem = new Ingredient()
            {
                Name = obj.Name,
                Description = obj.Description,
            };


            _context.Ingredient.Add(newItem); //Add to the database
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if (updated < 1) //determine that at least one item was added to the database
            {
                return NotFound();
            }

            return Json(newItem);
        }
        /// <summary>
        /// Retrieves a list of Ingredient objects from the database
        /// and returns that list to the caller.
        /// </summary>
        /// <returns>
        /// List of Ingredient objects
        /// </returns>
        [HttpGet]
        public ActionResult GetIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var ingredient in _context.Ingredient)
            {
                ingredients.Add(ingredient);
            }
            return Json(ingredients);
        }
        /// <summary>
        /// Get a list of menu item ingredients attached to a specific menu item
        /// then return that list.
        /// </summary>
        /// <param name="id">Menu item id</param>
        /// <returns>
        /// List of menu item ingredients
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> GetMenuItemIngredients(int id)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var item in _context.MenuItemIngredients)
            {
                if (item.MenuItemId == id)
                {

                    var ingredient = await _context.Ingredient.SingleOrDefaultAsync(m => m.Id == item.IngredientId);
                    ingredients.Add(ingredient);
                }


            }
            return Json(ingredients);
        }
        /// <summary>
        /// Map a new menu item ingredient to the specified menu item
        /// and add it to the database
        /// </summary>
        /// <param name="itemId">Menuitem Id</param>
        /// <param name="ingredientId">Ingredient Id</param>
        /// <returns>
        /// Ingredient object which has been mapped
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> AddMenuItemIngredient(int itemId, int ingredientId)
        {
            var item = await _context.MenuItem.FirstOrDefaultAsync(i => i.Id == itemId);
            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(i => i.Id == ingredientId);
            if (item == null)
            {
                return NotFound();
            }
            else if (ingredient == null)
            {
                return NotFound();
            }

            MenuItemIngredients ingredients = new MenuItemIngredients()
            {
                IngredientId = ingredientId,
                MenuItemId = itemId,
                Ingredient = ingredient,
                MenuItem = item
            };

            _context.MenuItemIngredients.Add(ingredients); //Add to the database
            var updated = await _context.SaveChangesAsync(); //Wait for database to update and get data
            if (updated < 1)
            {
                return NotFound();
            }
            return Json(ingredient);
        }
        /// <summary>
        /// Remove the mapping of an ingredient to a menu item
        /// and return true when it succeeds.
        /// </summary>
        /// <param name="itemId">Menuitem Id</param>
        /// <param name="ingredientId">Ingredient Id</param>
        /// <returns>
        /// true or false depending if successful
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> RemoveMenuItemIngredient(int itemId, int ingredientId)
        {
            MenuItemIngredients newMenuItemIngredient = new MenuItemIngredients()
            {
                MenuItemId = itemId,
                IngredientId = ingredientId
            };

            _context.MenuItemIngredients.Remove(newMenuItemIngredient);
            var update = await _context.SaveChangesAsync();
            if (update < 1)
            {
                return Json(false);
            }
            

            return Json(true);
        }
        /// <summary>
        /// Remove an ingredient from the database.
        /// </summary>
        /// <param name="id">Ingredient Id</param>
        /// <returns>
        /// True when Ingredient is removed
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> RemoveIngredient(int id)
        {
            var ingredient = await _context.Ingredient.SingleOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            _context.Ingredient.Remove(ingredient);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        /// <summary>
        /// Check if a category exists in the database.
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns>
        /// The context of the category.
        /// </returns>
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
