﻿using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class ReservedCredit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
