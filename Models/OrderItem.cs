using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApexVolley.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ID dell'ordine è obbligatorio.")]
        [Display(Name = "Ordine")]
        public int OrderId { get; set; }

        [Display(Name = "Ordine")]
        public Order Order { get; set; }

        [Required(ErrorMessage = "L'ID del prodotto è obbligatorio.")]
        [Display(Name = "Prodotto")]
        public int ProductId { get; set; }

        [Display(Name = "Prodotto")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "La quantità è obbligatoria.")]
        [Range(1, 1000, ErrorMessage = "La quantità deve essere compresa tra {1} e {2}.")]
        [Display(Name = "Quantità")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Il prezzo unitario è obbligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "Il prezzo unitario deve essere maggiore di 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Prezzo unitario")]
        public decimal UnitPrice { get; set; }
    }
}
