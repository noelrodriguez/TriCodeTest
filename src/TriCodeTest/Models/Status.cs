using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models
{
    public enum Status
    {
        Cart,
        Received,
        [Display(Name = "In Progress")]
        In_Progress,
        [Display(Name = "Pick Up")]
        Pick_Up,
        Completed
    }
}
