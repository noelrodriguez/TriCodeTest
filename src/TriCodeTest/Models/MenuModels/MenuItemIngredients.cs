using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.MenuModels
{
    public class MenuItemIngredients
    {
        public int MenuItemId { get; set; }
        public int IngredientId { get; set; }
        // Navigation Properties
        public MenuItem MenuItem { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
