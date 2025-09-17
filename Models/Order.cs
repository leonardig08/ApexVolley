using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApexVolley.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'utente è obbligatorio.")]
        [StringLength(450, ErrorMessage = "L'ID utente non può superare i {1} caratteri.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "La data di creazione è obbligatoria.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data ordine")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "L'importo totale è obbligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "L'importo totale deve essere maggiore di 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Totale")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Lo stato dell'ordine è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Lo stato non può superare i {1} caratteri.")]
        [Display(Name = "Stato")]
        public string Status { get; set; } = "Non pagato";

        [Required(ErrorMessage = "Il nome e cognome sono obbligatori.")]
        [StringLength(100, ErrorMessage = "Il nome e cognome non possono superare i {1} caratteri.")]
        [Display(Name = "Nome e Cognome")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
        [StringLength(200, ErrorMessage = "L'indirizzo non può superare i {1} caratteri.")]
        [Display(Name = "Indirizzo")]
        public string Address { get; set; }

        [Required(ErrorMessage = "La città è obbligatoria.")]
        [StringLength(100, ErrorMessage = "La città non può superare i {1} caratteri.")]
        [Display(Name = "Città")]
        public string City { get; set; }

        [Required(ErrorMessage = "Il CAP è obbligatorio.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Inserisci un CAP valido (5 cifre).")]
        [Display(Name = "CAP")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Il paese è obbligatorio.")]
        [StringLength(100, ErrorMessage = "Il paese non può superare i {1} caratteri.")]
        [Display(Name = "Paese")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Il numero di telefono è obbligatorio.")]
        [Phone(ErrorMessage = "Inserisci un numero di telefono valido.")]
        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "L'ID della sessione Stripe non può superare i {1} caratteri.")]
        [Display(Name = "Stripe Session ID")]
        public string? StripeSessionId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
