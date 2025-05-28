using System;
using System.ComponentModel.DataAnnotations;

namespace ApexVolley.Models
{
    public class NewsPost
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titolo")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Contenuto")]
        public string Content { get; set; }


        [Display(Name = "Data di pubblicazione")]
        [DataType(DataType.Date)]
        public DateTime? PublishedAt { get; set; }

        public string? MainImagePath { get; set; }
        public string? AdditionalImagePaths { get; set; } // Separati da ";"
        public string? AttachmentPaths { get; set; }
    }
}
