using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models
{
    public class CartCheckout
    {
        [Key]
        public string ItemId { get; set; }

        public string CartId { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int MenuItemId { get; set; }

        public virtual MenuModels.MenuItem MenuItem { get; set; }
    }
}
