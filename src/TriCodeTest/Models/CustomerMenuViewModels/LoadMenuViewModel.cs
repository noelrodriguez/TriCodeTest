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

    public class CartOrder
    {
        public int Id { get; set; }
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }
        [Display(Name = "Time")]
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
        // Navigation Properties
        public ApplicationUser User { get; set; }
        [Display(Name = "Items")]
        public List<CartOrderMenuItem> CartOrderMenuItems { get; set; }
    }

    public class CartOrderMenuItem
    {
        public MenuItem MenuItem { get; set; }
        public List<AddOn> AddOns { get; set; }
        public List<AddOn> ChoiceOfAddons { get; set; }
    }
}
