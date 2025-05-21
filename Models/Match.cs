using System;
using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class Match
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data della partita")]
        public DateTime Data { get; set; }

        [Display(Name = "Avversari")]
        public string? Avversari { get; set; }

        [Display(Name = "Luogo")]
        public string? Luogo { get; set; }

        [Display(Name = "Set 1")]
        public string? RisultatoSet1 { get; set; }

        [Display(Name = "Set 2")]
        public string? RisultatoSet2 { get; set; }

        [Display(Name = "Set 3")]
        public string? RisultatoSet3 { get; set; }

        [Display(Name = "Risultato finale")]
        public string? Risultato { get; set; }
    }
}
