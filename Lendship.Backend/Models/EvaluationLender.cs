using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class EvaluationLender
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UserFrom")]
        public string UserFromId { get; set; }

        [Required]
        public virtual ApplicationUser UserFrom { get; set; }

        [Required]
        [ForeignKey("UserTo")]
        public string UserToId { get; set; }

        [Required]
        public virtual ApplicationUser UserTo { get; set; }

        [Required]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Required]
        public virtual Advertisement Advertisement { get; set; }

        [Required]
        public int Flexibility { get; set; }

        [Required]
        public int Reliability { get; set; }

        [Required]
        public int QualityAtReturn { get; set; }

        [Required]
        public bool IsAnonymous { get; set; }

        [Required]
        public DateTime Creation { get; set; }
    }
}
