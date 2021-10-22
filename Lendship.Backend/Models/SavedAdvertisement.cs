using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class SavedAdvertisement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int AdvertisementId { get; set; }
    }
}
