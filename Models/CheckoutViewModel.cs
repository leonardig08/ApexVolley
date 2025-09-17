using ApexVolley.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CheckoutViewModel
{
    [ValidateNever]
    public List<CartItem> CartItems { get; set; }

    [Required, Display(Name = "Nome e Cognome")]
    public string FullName { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }    

    [Required, RegularExpression(@"^\d{5}$", ErrorMessage = "Inserisci un CAP valido (5 cifre).")]
    public string ZipCode { get; set; }

    [Required]
    public string Country { get; set; }

    [Required, Phone]
    public string PhoneNumber { get; set; }

}
