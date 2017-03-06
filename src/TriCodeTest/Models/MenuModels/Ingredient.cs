using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriCodeTest.Models.MenuModels
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //[NotMapped] Weston: I commented this out so ingredients can be added to the database.
        public Option Option { get; set; }
        // Navigation Properties
        [JsonIgnore]
        public List<MenuItemIngredients> MenuItemIngredients { get; set; }
    }
}
