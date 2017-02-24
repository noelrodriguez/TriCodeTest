using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Models.MenuModels;
using TriCodeTest.Models.OrderModels;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.CustomerMenuViewModels
{
    public class LoadMenuViewModel
    {
        [Display(Name = "Categories")]
        public List<Category> Categories { get; set; }
        [Display(Name = "Subcategories")]
        public List<Subcategory> Subcategories { get; set; }
        [Display(Name = "Items")]
        public List<MenuItem> MenuItems { get; set; }
    }
}
