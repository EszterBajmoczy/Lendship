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
        public string Location { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal EvaluationAsAdvertiser { get; set; }

        public int EvaluationAsAdvertiserCount { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal AdvertiserFlexibility { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal AdvertiserReliability { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal AdvertiserQualityOfProduct { get; set; }


        [Column(TypeName = "decimal(9,6)")]
        public decimal EvaluationAsLender { get; set; }

        public int EvaluationAsLenderCount { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal LenderFlexibility { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal LenderReliability { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal LenderQualityQualityAtReturn { get; set; }

        public string ImageLocation { get; set; }

        [Required]
        public DateTime Registration { get; set; }
    }
}
