using Microsoft.EntityFrameworkCore;
using UserRegister.Models;
using System;
using System.Linq;

namespace UserRegister.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly RegisterDbContext _dbContext;

        public RegisterRepository(RegisterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string RegisterUser(Register user)
        {
            // Check if passwords match
            if (user.Password != user.ConfirmPassword)
            {
                return "Passwords do not match.";
            }

            try
            {
                // Check for existing user
                if (_dbContext.Users.Any(u => u.Email == user.Email))
                {
                    return "Email already registered.";
                }

                var newUser = new Register
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password, // Store only the Password
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                return "Registration successful.";
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                return $"Registration failed: {innerException ?? "An unknown error occurred."}";
            }
            catch (Exception ex)
            {
                return "Registration failed: " + ex.Message;
            }
        }

        public Register GetUserByEmail(string email)
        {
            return _dbContext.Users.Find(email);
        }

        public string ValidateUser(string email, string password, string confirmPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return "User not found.";
            }

            // Check if the password matches
            if (user.Password == password && password == confirmPassword) // Include confirmPassword check
            {
                return "Validation successful.";
            }
            return "Validation failed.";
        }

        public string UpdateUserDetails(Register user)
        {
            try
            {
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email); // Adjusting to find by Email
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.DateOfBirth = user.DateOfBirth;
                    existingUser.Gender = user.Gender;
                    _dbContext.SaveChanges();
                    return "User update successful.";
                }
                return "User not found.";
            }
            catch (Exception ex)
            {
                return "User update failed: " + ex.Message;
            }
        }

        public string UpdateUserPassword(string email, string password)
        {
            try
            {
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == email); // Adjusting to find by Email
                if (existingUser != null)
                {
                    existingUser.Password = password;
                    _dbContext.SaveChanges();
                    return "User password update successful.";
                }
                return "User not found.";
            }
            catch (Exception ex)
            {
                return "User update failed: " + ex.Message;
            }
        }
        public string DeleteUser(string email)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    _dbContext.SaveChanges();
                    return "User deletion successful.";
                }
                return "User not found.";
            }
            catch (Exception ex)
            {
                return "User deletion failed: " + ex.Message;
            }
        }

    }
}
