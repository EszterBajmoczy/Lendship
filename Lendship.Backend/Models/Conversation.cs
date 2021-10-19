﻿using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Advertisement Advertisement { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
