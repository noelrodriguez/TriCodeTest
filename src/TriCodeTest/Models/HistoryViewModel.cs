using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TriCodeTest.Models.OrderModels;

namespace TriCodeTest.Models
{
    public class HistoryViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<OrderInfo> Orders { get; set; }
    }

    public class PostHistoryViewModel
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
    }
}