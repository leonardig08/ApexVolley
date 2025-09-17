using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'utente è obbligatorio.")]
        [StringLength(450, ErrorMessage = "L'ID utente non può superare i {1} caratteri.")]
        public string UserId { get; set; }  // ID utente loggato (Identity usa fino a 450 caratteri per le chiavi string)

        [Required(ErrorMessage = "Il prodotto è obbligatorio.")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required(ErrorMessage = "La quantità è obbligatoria.")]
        [Range(1, 100, ErrorMessage = "La quantità deve essere compresa tra {1} e {2}.")]
        public int Quantity { get; set; } = 1;
    }
}
