using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class ImageLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
