namespace ApexVolley.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Avversari { get; set; }
        public string Luogo { get; set; }
        public string Risultato { get; set; }
        public string RisultatoSet1 { get; set; }
        public string RisultatoSet2 { get; set; }
        public string RisultatoSet3 { get; set; }
    }
}
