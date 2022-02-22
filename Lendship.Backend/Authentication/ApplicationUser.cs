using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lendship.Backend.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int Credit { get; set; }

        [Required]
        public bool EmailNotificationsEnabled { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,6)")]
        public decimal Latitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        public decimal Longitude { get; set; }

        [Required]
        public DateTime Registration { get; set; }
    }
}
