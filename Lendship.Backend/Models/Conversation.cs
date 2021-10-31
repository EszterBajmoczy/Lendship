using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Advertisement Advertisement { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Guid> UserIds { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
