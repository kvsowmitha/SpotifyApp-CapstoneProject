using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserRegister.Models
{
    public class Register
    {
        [Key]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [NotMapped] // Make sure to add this attribute
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        public string Gender { get; set; }

    }
}
