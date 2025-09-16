using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Nome e Cognome")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Indirizzo")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Città")]
        public string City { get; set; }

        [Required]
        [Display(Name = "CAP")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Paese")]
        public string Country { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }
    }
}
