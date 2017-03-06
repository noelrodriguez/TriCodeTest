using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TriCodeTest.Models.MenuModels
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public byte[] ItemImage { get; set; }
        [NotMapped]
        public Size? Size { get; set; }
        [Required]
        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }
        [JsonIgnore]
        // Navigation Properties
        public Subcategory Subcategory { get; set; }
        public List<MenuItemIngredients> MenuItemIngredients { get; set; }
    }
}
