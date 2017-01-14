using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriCodeTest.Models.MenuModels;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderMenuItem
    {
        public MenuItem MenuItem { get; set; }
        public List<AddOn> AddOns { get; set; }
    }
}
