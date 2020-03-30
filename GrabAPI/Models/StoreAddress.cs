using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models
{
    public class StoreAddress
    {
        public int Id { get; set; }
        public Nullable<int> Unit { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public Nullable<int> StoreID { get; set; }

        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

    }
}
