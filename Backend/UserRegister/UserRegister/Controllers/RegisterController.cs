using Microsoft.AspNetCore.Mvc;
using UserRegister.Models;
using UserRegister.Repository;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace UserRegister.Controllers
{
    [ApiController]
    [Route("api/register")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository _registerRepository;

        public RegisterController(IRegisterRepository registerRepository)
        {
            _registerRepository = registerRepository;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] Register user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User data is required." });
            }

            try
            {
                var result = _registerRepository.RegisterUser(user);

                if (result.Contains("successful"))
                {
                    return Ok(new { message = "User registration successful." });
                }

                return Conflict(new { message = "User registration failed: " + result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                var user = _registerRepository.GetUserByEmail(email);
                if (user != null)
                {
                    return Ok(user);
                }

                return NotFound(new { message = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("updatePassword")]
        public IActionResult UpdatePassword(string email, string password)
        {
            if (email == null || password == null)
            {
                return BadRequest(new { message = "User data is required." });
            }
            try
            {
                var result = _registerRepository.UpdateUserPassword(email, password);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }

        }

        [HttpPost("validate")]
        public IActionResult ValidateUser([FromBody] ValidateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var user = _registerRepository.GetUserByEmail(request.Email);
            if (user == null || user.Password != request.Password)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            return Ok(user); // Optionally return user info or just a success message
        }

    [HttpPut("update")]
        public IActionResult UpdateUserDetails([FromBody] Register user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User data is required." });
            }

            try
            {
                var result = _registerRepository.UpdateUserDetails(user);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("email/{email}")]
        public IActionResult DeleteUser(string email)
        {
            try
            {
                var result = _registerRepository.DeleteUser(email);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

    public class ValidateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
    }
}
}
