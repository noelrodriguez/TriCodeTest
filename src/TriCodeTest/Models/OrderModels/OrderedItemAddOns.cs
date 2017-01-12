using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderedItemAddOns
    {
        [Key]
        public int OrderedItemId { get; set; }
        public int OrderedAddOnId { get; set; }
        public OrderedItem OrderedItem { get; set; }
        public OrderedAddOn OrderedAddOn { get; set; }
    }
}
