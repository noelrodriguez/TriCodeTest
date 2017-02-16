using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TriCodeTest.Models.MenuModels
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        // Navigation Properties
        [JsonIgnore]
        public Category Category { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List<AddOn> AddOns { get; set; }
    }
}
