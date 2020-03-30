using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models.ViewModel
{
    public class CartDetails
    {
        public List<Cart> listCart { get; set; }
        public Order Order { get; set; }
    }
}
