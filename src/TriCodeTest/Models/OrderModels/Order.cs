using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Models;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        public Status Status { get; set; }
        public DateTime DateTime { get; set; }
        public List<OrderOrderedItems> OrderOrderedItems { get; set; }
    }
}
