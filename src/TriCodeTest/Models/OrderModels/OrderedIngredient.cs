using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderedIngredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Option Option { get; set; }
        public List<OrderedItemIngredients> OrderedItemIngredients { get; set; }
    }
}
