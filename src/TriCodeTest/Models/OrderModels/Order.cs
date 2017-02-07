using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Models.MenuModels;

namespace TriCodeTest.Models.OrderModels
{
    public class Order
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
        // Navigation Properties
        public ApplicationUser User { get; set; }
        public List<OrderMenuItem> OrderMenuItems { get; set; }
    }
}
