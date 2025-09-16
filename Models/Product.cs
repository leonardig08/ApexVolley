using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(0.01, 100000, ErrorMessage = "Il prezzo deve essere maggiore di 0")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; } // opzionale, verrà salvato il percorso relativo
    }
}
