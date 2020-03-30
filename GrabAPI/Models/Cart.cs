using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models
{
    public class Cart
    {
      
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        public int Count { get; set; }
    }
}
