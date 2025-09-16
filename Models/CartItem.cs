using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }  // ID utente loggato

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
