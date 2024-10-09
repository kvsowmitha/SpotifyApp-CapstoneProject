using System;
using UserRegister.Models;
using UserRegister.Repository;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Exceptions;

namespace UserRegister.Services
{
    public class UserServices : IUserService
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly KafkaSettings _kafkaSettings;

        public UserServices(IRegisterRepository registerRepository, IOptions<KafkaSettings> kafkaSettings)
        {
            _registerRepository = registerRepository;
            _kafkaSettings = kafkaSettings.Value;
        }
        public string RegisterUser(Register user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User cannot be null.");
                }

                // Check if passwords match
                if (user.Password != user.ConfirmPassword)
                {
                    return "Passwords do not match.";
                }

                var result = _registerRepository.RegisterUser(user);

                if (result.Contains("successful"))
                {
                    // Send a message to Kafka
                    var message = new Message<string, string>
                    {
                        Key = user.Email,
                        Value = "User registered successfully."
                    };

                    using (var producer = new ProducerBuilder<string, string>(new ProducerConfig
                    {
                        BootstrapServers = _kafkaSettings.BootstrapServers
                    }).Build())
                    {
                        producer.Produce(_kafkaSettings.TopicName, message);
                    }
                }

                return result;
            }
            catch (ArgumentNullException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return $"Error registering user: {ex.Message}";
            }
        }
        public Register GetUserByEmail(string email)
        {
            return _registerRepository.GetUserByEmail(email);
        }
        public string ValidateUser(string email, string password, string confirmPassword)
        {
            try
            {
                string validationMessage = _registerRepository.ValidateUser(email, password, confirmPassword);
                return validationMessage;
            }
            catch (Exception ex)
            {
                return $"Error validating user: {ex.Message}";
            }
        }
        public string UpdateUserDetails(Register user)
        {
            try
            {
                var existingUser = _registerRepository.GetUserByEmail(user.Email);
                if (existingUser == null)
                {
                    throw new UserNotFoundException($"User with Email {user.Email} not found.");
                }

                existingUser.Name = user.Name;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.Gender = user.Gender;

                _registerRepository.UpdateUserDetails(existingUser);
                return "User update successful.";
            }
            catch (UserNotFoundException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return $"Error updating user: {ex.Message}";
            }
        }
        public string DeleteUser(string email)
        {
            try
            {
                var result = _registerRepository.DeleteUser(email);
                return result;
            }
            catch (Exception ex)
            {
                return $"Error deleting user: {ex.Message}";
            }
        }
    }
}
