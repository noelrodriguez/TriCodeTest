using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderedItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public Size? Size { get; set; }
        public List<OrderOrderedItems> OrderOrderedItems { get; set; }
        public List<OrderedItemAddOns> OrderedItemAddOns { get; set; }
        public List<OrderedItemIngredients> OrderedItemIngredients { get; set; }
    }
}
