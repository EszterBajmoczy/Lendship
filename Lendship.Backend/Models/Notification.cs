using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        [Required]
        public string AdvertisementTitle { get; set; }

        [Required]
        public DateTime ReservationDateFrom { get; set; }

        [Required]
        public DateTime ReservationDateTo { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public string UpdateInformation { get; set; }

        [Required]
        public bool New { get; set; }

        [Required]
        public DateTime TimeSpan { get; set; }
    }
}
