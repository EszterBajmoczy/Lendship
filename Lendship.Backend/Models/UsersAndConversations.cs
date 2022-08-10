using Lendship.Backend.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class UsersAndConversations
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ConversationId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Conversation Conversation { get; set; }
    }
}
