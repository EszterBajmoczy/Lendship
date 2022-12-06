using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class EvaluationAdvertiser: Evaluation
    {
        [Required]
        public int QualityOfProduct { get; set; }
    }
}
