using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApexVolley.Models {
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Non pagato";

        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string ?StripeSessionId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
