using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderOrderedItems
    {
        [Key]
        public int OrderId { get; set; }
        public int OrderedItemId { get; set; }
        public Order Order { get; set; }
        public OrderedItem OrderedItem { get; set; }
    }
}
