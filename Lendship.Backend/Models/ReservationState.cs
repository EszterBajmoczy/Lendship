﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class ReservationState
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string State { get; set; }
    }
}
