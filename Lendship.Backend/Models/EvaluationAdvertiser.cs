using Lendship.Backend.Authentication;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class EvaluationAdvertiser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationUser UserFrom { get; set; }

        [Required]
        public  ApplicationUser UserTo { get; set; }

        [Required]
        public Advertisement Advertisement { get; set; }

        [Required]
        public int Flexibility { get; set; }

        [Required]
        public int Reliability { get; set; }

        [Required]
        public int QualityOfProduct { get; set; }

        public string Comment { get; set; }

        [Required]
        public bool IsAnonymous { get; set; }

        [Required]
        public DateTime Creation { get; set; }
    }
}
