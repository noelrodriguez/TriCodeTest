using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.OrderModels
{
    public class OrderInfo
    {
        public int Id { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
        // Navigation Properties
        public ApplicationUser User { get; set; }
        // JSON Object
        public string OrderMenuItems { get; set; }
    }
}
