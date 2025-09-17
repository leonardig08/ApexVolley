using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Lo username è obbligatorio.")]
    [StringLength(50, ErrorMessage = "Lo username non può superare i {1} caratteri.")]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "L'email è obbligatoria.")]
    [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La password è obbligatoria.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La password deve essere lunga almeno {2} caratteri.")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Conferma la password.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Le password non coincidono.")]
    [Display(Name = "Conferma Password")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Il nome è obbligatorio.")]
    [StringLength(50, ErrorMessage = "Il nome non può superare i {1} caratteri.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Il cognome è obbligatorio.")]
    [StringLength(50, ErrorMessage = "Il cognome non può superare i {1} caratteri.")]
    [Display(Name = "Cognome")]
    public string Cognome { get; set; }
}
