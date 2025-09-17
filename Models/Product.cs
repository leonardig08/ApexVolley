using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome del prodotto è obbligatorio.")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i {1} caratteri.")]
        [Display(Name = "Nome prodotto")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descrizione del prodotto è obbligatoria.")]
        [StringLength(2000, ErrorMessage = "La descrizione non può superare i {1} caratteri.")]
        [Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il prezzo del prodotto è obbligatorio.")]
        [Range(0.01, 100000, ErrorMessage = "Il prezzo deve essere maggiore di 0.")]
        [Display(Name = "Prezzo (€)")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(300, ErrorMessage = "Il percorso dell'immagine non può superare i {1} caratteri.")]
        [Display(Name = "Immagine")]
        public string? ImageUrl { get; set; } // opzionale, verrà salvato il percorso relativo
    }
}
