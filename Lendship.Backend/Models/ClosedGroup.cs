using Lendship.Backend.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Models
{
    public class ClosedGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Advertisement Advertisement { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
