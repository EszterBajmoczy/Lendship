using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Required]
        public virtual Advertisement Advertisement { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }
    }
}
