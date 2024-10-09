using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AuthenticationService.Model; 
using AuthenticationService.Repository;
using AuthenticationService.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json; 
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer; 


namespace AuthenticationService.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IAuthRepo _authRepo; 

        public AuthController(IConfiguration configuration,IAuthRepo authRepo)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _authRepo = authRepo;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginData user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            // Validate against the external service
            var validateRequest = new
            {
                Email = user.Email,
                Password = user.Password
            };

            var json = JsonSerializer.Serialize(validateRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5290/api/register/validate", content);

            if (response.IsSuccessStatusCode)
            {
                var registeredUserJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Registered User JSON: " + registeredUserJson);
                var registeredUser = JsonSerializer.Deserialize<Register>(registeredUserJson, new JsonSerializerOptions
                {
                    Converters = { new DateTimeConverter() }
                });

               
                if (registeredUser == null || string.IsNullOrEmpty(registeredUser.email))
                {
                    return BadRequest(new { message = "User data is invalid." });
                }

                // Check if the user already exists in the database
                //var existingUser = await _authRepo.GetUserByEmail(user.Email);

                if (registeredUserJson != null)
                {

                    // Parse the JSON into a C# object
                    var registeredUserData = JsonConvert.DeserializeObject<Register>(registeredUserJson);

                    // Validate the password (you should hash and compare in production)
                    if (registeredUserData.password != user.Password)
                    {
                        return Unauthorized(new { message = "Invalid credentials." });
                    }

                    // Generate token and return it
                    string tokenString = GenerateToken(registeredUser);
                    return Ok(new { token = tokenString });
                }

                // If user does not exist, save new user data
                var loginData = new LoginData
                {
                    Email = registeredUser.email,
                    Password = user.Password 
                };

                if (!_authRepo.AddUserData(loginData))
                {
                    return StatusCode(500, new { message = "Failed to save user data." });
                }

                // Generate token for new user
                string newTokenString = GenerateToken(registeredUser);
                return Ok(new { token = newTokenString, user = registeredUser.name});
            }
            // Handle invalid responses from the external validation service
            Console.WriteLine($"External validation failed for {user.Email}. Response status: {response.StatusCode}");
            return Unauthorized(new { message = "Invalid credentials." });
        }

        private string GenerateToken(Register user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            // Generate symmetric security key from the JWT configuration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Define claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.email ?? string.Empty), // Ensure email is not null
        
            };

            // Set up the token handler and token descriptor with issuer, audience, and signing credentials
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],       // ValidateIssuer
                Audience = _configuration["Jwt:Audience"],   // ValidateAudience
            };

            // Create and write the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordResetRequest request)
        {
            var otp = await _authRepo.GenerateOtpForPasswordReset(request.Email);

            if (otp == null)
            {
                return BadRequest(new { message = "Email not found." });
            }

            return Ok(new { message = "OTP sent to your email." });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetConfirmation confirmation)
        {
            if (confirmation.NewPassword != confirmation.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match." });
            }

            // Validate the OTP
            if (!await _authRepo.ValidateOtp(confirmation.Email, confirmation.Otp))
            {
                return BadRequest(new { message = "Invalid OTP." });
            }

            // Call the User Registration API to update the password
            using (var httpClient = new HttpClient())
            {
                // Create the API endpoint URL with query parameters
                var url = $"http://localhost:5290/api/register/updatePassword?email={confirmation.Email}&password={confirmation.NewPassword}";

                // Make the request
                var response = await httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to update password in User Registration API." });
                }
            }

            // Send confirmation email
            SendEmail(confirmation.Email, "Password Updated", "Your password has been successfully updated.");

            return Ok(new { message = "Password updated successfully." });
        }

        private void SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587; // Use appropriate port
                client.Credentials = new NetworkCredential("kvsowmitha@gmail.com", "bwiyescxjujaadox");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("kvsowmitha@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);
                client.Send(mailMessage);
            }
        }
    [HttpGet("{emailId}")]
        public async Task<IActionResult> GetUserByemailId(string emailId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5290/api/register/email/{emailId}");
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<Register>(await response.Content.ReadAsStringAsync());
                return Ok(user);
            }

            return NotFound(new { message = "User not found." });
        }
    }
}
