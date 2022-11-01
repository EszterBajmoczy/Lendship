using Lendship.Backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
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
        public int ReservedCredit { get; set; }

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

        public double EvaluationAsAdvertiser { get; set; }

        public int EvaluationAsAdvertiserCount { get; set; }

        public double AdvertiserFlexibility { get; set; }

        public double AdvertiserReliability { get; set; }

        public double AdvertiserQualityOfProduct { get; set; }

        public double EvaluationAsLender { get; set; }

        public int EvaluationAsLenderCount { get; set; }

        public double LenderFlexibility { get; set; }

        public double LenderReliability { get; set; }

        public double LenderQualityAtReturn { get; set; }

        public string ImageLocation { get; set; }

        [Required]
        public DateTime Registration { get; set; }
    }
}
