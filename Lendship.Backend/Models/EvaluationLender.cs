using Lendship.Backend.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class EvaluationLender : Evaluation
    {
        [Required]
        public int QualityAtReturn { get; set; }
    }
}
