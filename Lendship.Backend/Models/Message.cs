using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationUser UserFrom { get; set; }

        [Required]
        public int ConversationId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
