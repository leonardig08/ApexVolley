using System;
using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class NewsPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio.")]
        [StringLength(150, ErrorMessage = "Il titolo non può superare i {1} caratteri.")]
        [Display(Name = "Titolo")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Il contenuto è obbligatorio.")]
        [StringLength(5000, ErrorMessage = "Il contenuto non può superare i {1} caratteri.")]
        [Display(Name = "Contenuto")]
        public string Content { get; set; }

        [Display(Name = "Data di pubblicazione")]
        [DataType(DataType.Date)]
        public DateTime? PublishedAt { get; set; }

        [StringLength(300, ErrorMessage = "Il percorso dell'immagine principale non può superare i {1} caratteri.")]
        [Display(Name = "Immagine principale")]
        public string? MainImagePath { get; set; }

        [StringLength(1000, ErrorMessage = "I percorsi delle immagini aggiuntive non possono superare i {1} caratteri.")]
        [Display(Name = "Immagini aggiuntive (separate da ;)")]
        public string? AdditionalImagePaths { get; set; }

        [StringLength(1000, ErrorMessage = "I percorsi degli allegati non possono superare i {1} caratteri.")]
        [Display(Name = "Allegati")]
        public string? AttachmentPaths { get; set; }
    }
}
