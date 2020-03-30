using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models.ViewModel
{
    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
