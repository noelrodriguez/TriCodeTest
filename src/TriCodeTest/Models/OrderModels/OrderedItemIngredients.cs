using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderedItemIngredients
    {
        [Key]
        public int OrderedItemId { get; set; }
        public int OrderedIngredientId { get; set; }
        public OrderedItem OrderedItem { get; set; }
        public OrderedIngredient OrderedIngredient { get; set; }
    }
}
