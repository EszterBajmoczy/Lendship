using Lendship.Backend.Authentication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lendship.Backend.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Required]
        public virtual Advertisement Advertisement { get; set; }

        [Required]
        [ForeignKey("ReservationState")]
        public int ReservationStateId { get; set; }

        public virtual ReservationState ReservationState { get; set; }

        public string Comment { get; set; }

        public bool admittedByAdvertiser { get; set; }

        public bool admittedByLender { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }
    }
}
