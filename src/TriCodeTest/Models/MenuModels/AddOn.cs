﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TriCodeTest.Models.MenuModels
{
    public class AddOn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
