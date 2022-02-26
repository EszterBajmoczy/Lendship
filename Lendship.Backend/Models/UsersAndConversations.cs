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
    }
}
