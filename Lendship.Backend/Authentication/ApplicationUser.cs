using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int Credit { get; set; }

        [Required]
        public bool EmailNotificationsEnabled { get; set; }

        [Required]
        [DataType("decimal(8,6)")]
        public int Latitude { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Longitude { get; set; }

        [Required]
        public DateTime Registration { get; set; }
    }
}
