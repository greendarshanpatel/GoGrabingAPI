using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
    }
}
