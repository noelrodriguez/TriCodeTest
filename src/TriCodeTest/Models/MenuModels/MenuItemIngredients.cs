using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TriCodeTest.Models.MenuModels
{
    public class MenuItemIngredients
    {
        public int MenuItemId { get; set; }
        public int IngredientId { get; set; }
        // Navigation Properties
        [JsonIgnore]
        public MenuItem MenuItem { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
