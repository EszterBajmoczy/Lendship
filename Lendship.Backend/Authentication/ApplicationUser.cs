﻿using Lendship.Backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
        public string Location { get; set; }

        public string ImageLocation { get; set; }

        [Required]
        public DateTime Registration { get; set; }

        [Required]
        public EvaluationComputed Evaluation { get; set; }

        public virtual ICollection<ReservedCredit> ReservedCredits { get; set; }
    }
}
