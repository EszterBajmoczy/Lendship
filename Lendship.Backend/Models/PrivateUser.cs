using Lendship.Backend.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class PrivateUser
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        [Required]
        public virtual Advertisement Advertisement { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
