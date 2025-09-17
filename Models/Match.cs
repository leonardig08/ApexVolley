using System;
using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class Match
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La data della partita è obbligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data della partita")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Gli avversari sono obbligatori.")]
        [StringLength(100, ErrorMessage = "Il nome degli avversari non può superare i {1} caratteri.")]
        [Display(Name = "Avversari")]
        public string Avversari { get; set; }

        [Required(ErrorMessage = "Il luogo è obbligatorio.")]
        [StringLength(150, ErrorMessage = "Il luogo non può superare i {1} caratteri.")]
        [Display(Name = "Luogo")]
        public string Luogo { get; set; }

        [StringLength(20, ErrorMessage = "Il risultato finale non può superare i {1} caratteri.")]
        [Display(Name = "Risultato finale")]
        public string? Risultato { get; set; }

        [StringLength(10, ErrorMessage = "Il risultato del set non può superare i {1} caratteri.")]
        [Display(Name = "Set 1")]
        public string? RisultatoSet1 { get; set; }

        [StringLength(10, ErrorMessage = "Il risultato del set non può superare i {1} caratteri.")]
        [Display(Name = "Set 2")]
        public string? RisultatoSet2 { get; set; }

        [StringLength(10, ErrorMessage = "Il risultato del set non può superare i {1} caratteri.")]
        [Display(Name = "Set 3")]
        public string? RisultatoSet3 { get; set; }
    }
}
