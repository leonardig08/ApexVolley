using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class ErrorViewModel
    {
        [StringLength(100, ErrorMessage = "L'ID della richiesta non può superare i {1} caratteri.")]
        [Display(Name = "ID Richiesta")]
        public string? RequestId { get; set; }

        [Display(Name = "Mostra ID richiesta")]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
