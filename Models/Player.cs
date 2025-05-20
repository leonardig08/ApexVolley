using System.ComponentModel.DataAnnotations;
namespace ApexVolley.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataNascita { get; set; }

        public string? Ruolo { get; set; } // Es: Palleggiatore, Centrale, Schiacciatore
        public int AltezzaCm { get; set; }
        public int NumeroMaglia { get; set; }
    }
}
