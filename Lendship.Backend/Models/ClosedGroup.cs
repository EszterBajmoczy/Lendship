using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class ClosedGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdvertismentId { get; set; }
    }
}
