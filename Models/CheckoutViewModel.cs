using ApexVolley.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CheckoutViewModel
{
    [ValidateNever]
    public List<CartItem> CartItems { get; set; }

    [Required(ErrorMessage = "Il nome e cognome sono obbligatori.")]
    [StringLength(100, ErrorMessage = "Il nome e cognome non possono superare i {1} caratteri.")]
    [Display(Name = "Nome e Cognome")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
    [StringLength(200, ErrorMessage = "L'indirizzo non può superare i {1} caratteri.")]
    [Display(Name = "Indirizzo")]
    public string Address { get; set; }

    [Required(ErrorMessage = "La città è obbligatoria.")]
    [StringLength(100, ErrorMessage = "La città non può superare i {1} caratteri.")]
    [Display(Name = "Città")]
    public string City { get; set; }

    [Required(ErrorMessage = "Il CAP è obbligatorio.")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Inserisci un CAP valido (5 cifre).")]
    [Display(Name = "CAP")]
    public string ZipCode { get; set; }

    [Required(ErrorMessage = "Il paese è obbligatorio.")]
    [StringLength(100, ErrorMessage = "Il paese non può superare i {1} caratteri.")]
    [Display(Name = "Paese")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Il numero di telefono è obbligatorio.")]
    [Phone(ErrorMessage = "Inserisci un numero di telefono valido.")]
    [Display(Name = "Telefono")]
    public string PhoneNumber { get; set; }
}
