using Lendship.Backend.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public enum ReservationState
    {
        Created,
        Accepted,
        Declined,
        Resigned,
        Ongoing,
        Closed
    }

    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public  Advertisement Advertisement { get; set; }

        public ReservationState ReservationState { get; set; }

        public string Comment { get; set; }

        public bool admittedByAdvertiser { get; set; }

        public bool admittedByLender { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }
    }
}
