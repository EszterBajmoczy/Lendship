using System.ComponentModel.DataAnnotations;

namespace Lendship.Backend.DTO.Authentication
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The two passwords does not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
