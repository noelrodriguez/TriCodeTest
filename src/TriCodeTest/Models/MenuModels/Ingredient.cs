using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TriCodeTest.Models.MenuModels
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Option Option { get; set; }
        // Navigation Properties
        [JsonIgnore]
        public List<MenuItemIngredients> MenuItemIngredients { get; set; }
    }
}
