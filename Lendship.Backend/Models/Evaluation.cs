using Lendship.Backend.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationUser UserFrom { get; set; }

        [Required]
        public  ApplicationUser UserTo { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int Flexibility { get; set; }

        [Required]
        public int Reliability { get; set; }

        public string Comment { get; set; }

        [Required]
        public bool IsAnonymous { get; set; }

        [Required]
        public DateTime Creation { get; set; }
    }
}
