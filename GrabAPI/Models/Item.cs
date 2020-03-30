using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public double Cost { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int StoreId { get; set; }

        [ForeignKey("ItemTypeID")]
        public virtual ItemType ItemType { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }
}
