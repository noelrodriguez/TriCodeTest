using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderedAddOn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public List<OrderedItemAddOns> OrderedItemAddOns { get; set; }
    }
}
