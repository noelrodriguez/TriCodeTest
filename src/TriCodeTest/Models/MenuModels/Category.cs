using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TriCodeTest.Models.MenuModels
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // Navigation Properties
        public List<Subcategory> Subcategories { get; set; }
    }
}
