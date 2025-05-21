using System.ComponentModel.DataAnnotations;
namespace ApexVolley.Models
{
    public class Player
    {
        public int Id { get; set; }

        
        public string? Nome { get; set; }
        public string? Cognome { get; set; }

        [DataType(DataType.Date), Display(Name ="Data di Nascita")]
        public DateTime DataNascita { get; set; }

        public string? Ruolo { get; set; }

        [Display(Name ="Altezza (cm)")]
        public int AltezzaCm { get; set; }
        [Display(Name ="Numero di maglia")]
        public int NumeroMaglia { get; set; }
    }
}
