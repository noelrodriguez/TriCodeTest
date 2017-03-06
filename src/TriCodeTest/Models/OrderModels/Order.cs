using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Models.MenuModels;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class Order
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
        public List<OrderMenuItem> OrderMenuItems { get; set; }
    }
}
