using Lendship.Backend.Authentication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lendship.Backend.Models
{
    public class Advertisement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string InstructionManual { get; set; }

        public int? Price { get; set; }
        
        public int? Credit { get; set; }

        public int? Deposit { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public DateTime Creation { get; set; }


    }
}
