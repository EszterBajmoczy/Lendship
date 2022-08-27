using System;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }
    }
}
