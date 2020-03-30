using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Image { get; set; }
        public int AccountType { get; set; }
        public Nullable<int> Unit { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        [ForeignKey("AccountType")]
        public virtual AccountType UserAccountType { get; set; }
    }
}
