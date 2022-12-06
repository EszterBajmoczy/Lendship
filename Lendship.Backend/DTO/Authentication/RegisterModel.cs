using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.DTO.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        public decimal Longitude { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }     
    }
}
