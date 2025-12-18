using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.DTOs
{
    public class RegisterDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }
    }
}
