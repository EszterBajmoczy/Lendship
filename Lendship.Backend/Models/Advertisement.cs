﻿using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public enum AdvertisementType
    {
        Property,
        Service
    }
    public class Advertisement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public AdvertisementType AdvertisementType { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string InstructionManual { get; set; }

        public int? Price { get; set; }
        
        public int? Credit { get; set; }

        public int? Deposit { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public DateTime Creation { get; set; }

        public ICollection<ImageLocation> ImageLocations { get; set; }

        public ICollection<Availability> Availabilities { get; set; }

        public virtual ICollection<PrivateUser> PrivateUsers { get; set; }
    }
}
