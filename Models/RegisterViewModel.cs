using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Le password non coincidono.")]
    public string ConfirmPassword { get; set; }

    public string Nome { get; set; }
    public string Cognome { get; set; }
}
