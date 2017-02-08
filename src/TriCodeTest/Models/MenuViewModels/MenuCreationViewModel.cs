using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Models.MenuModels;

namespace TriCodeTest.Models.MenuViewModels
{
    public class MenuCreationViewModel
    {
        public List<Category> CategoryMenu { get; set; }

        public List<Subcategory> SubCategoryMenu { get; set; }

        public List<MenuItem> MenuItemMenu { get; set; }

        public List<MenuItemIngredients> MenuItemIngredientsMenu { get; set; }

        public List<AddOn> AddonMenu { get; set; }

        public List<Ingredient> IngridientMenu { get; set; }
    }
}
