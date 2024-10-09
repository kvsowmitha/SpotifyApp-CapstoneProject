using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Model
{
    public class LoginData
    {
        [Key]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
