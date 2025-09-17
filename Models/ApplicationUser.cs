using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Il nome è obbligatorio.")]
    [StringLength(50, ErrorMessage = "Il nome non può superare i {1} caratteri.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Il cognome è obbligatorio.")]
    [StringLength(50, ErrorMessage = "Il cognome non può superare i {1} caratteri.")]
    public string Cognome { get; set; }

    [Required(ErrorMessage = "Il ruolo è obbligatorio.")]
    [StringLength(100, ErrorMessage = "Il ruolo non può superare i {1} caratteri.")]
    public string RuoloCompleto { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La data di registrazione è obbligatoria.")]
    public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;
}
