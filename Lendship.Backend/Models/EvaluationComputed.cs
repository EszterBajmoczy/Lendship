using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.Models
{
    public class EvaluationComputed
    {
        [Key]
        public int Id { get; set; }

        public double EvaluationAsAdvertiser { get; set; }

        public int EvaluationAsAdvertiserCount { get; set; }

        public double AdvertiserFlexibility { get; set; }

        public double AdvertiserReliability { get; set; }

        public double AdvertiserQualityOfProduct { get; set; }

        public double EvaluationAsLender { get; set; }

        public int EvaluationAsLenderCount { get; set; }

        public double LenderFlexibility { get; set; }

        public double LenderReliability { get; set; }

        public double LenderQualityAtReturn { get; set; }
    }
}
