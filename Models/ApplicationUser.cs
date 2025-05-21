using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

public class ApplicationUser : IdentityUser
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string RuoloCompleto { get; set; }
    public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;
}
