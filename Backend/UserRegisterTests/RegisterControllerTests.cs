using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UserRegister.Controllers;
using UserRegister.Models;
using UserRegister.Repository;
using Newtonsoft.Json.Linq;

namespace UserRegister.Tests
{
    [TestFixture]
    public class RegisterControllerTests
    {
        private Mock<IRegisterRepository> _mockRepository;
        private RegisterController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRegisterRepository>();
            _controller = new RegisterController(_mockRepository.Object);
        }

        [Test]
        public void RegisterUser_UserIsNull_ReturnsBadRequest()
        {
            // Arrange
            Register user = null;

            // Act
            var result = _controller.RegisterUser(user);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void RegisterUser_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var user = new Register
            {
                Email = "test@example.com",
                Name = "Test User",
                Password = "password123",
                ConfirmPassword = "password123",
                DateOfBirth = DateTime.Now, // Set a valid date for the user
                Gender = "Male"
            };

            // Setting up the mock to return a string
            _mockRepository.Setup(repo => repo.RegisterUser(It.IsAny<Register>())).Returns("User registration successful."); // Match the actual message

            // Act
            var result = _controller.RegisterUser(user) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Accessing the result.Value
            var value = result.Value;
            Assert.IsNotNull(value);

            // Use reflection to check the value type
            var messageProperty = value.GetType().GetProperty("message");

            if (messageProperty != null)
            {
                // If the property exists, get its value
                var messageValue = messageProperty.GetValue(value, null) as string;
                Assert.AreEqual("User registration successful.", messageValue); // Check against the actual return
            }
            else
            {
                // If not an anonymous object, check the string directly
                Assert.IsInstanceOf<string>(value);
                Assert.AreEqual("User registration successful.", value);
            }
        }



        [Test]
        public void ValidateUser_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var request = new RegisterController.ValidateRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new Register
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Setting up the mock to return the user
            _mockRepository.Setup(repo => repo.GetUserByEmail(request.Email)).Returns(user);

            // Act
            var result = _controller.ValidateUser(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<Register>(result.Value);
            var returnedUser = result.Value as Register;
            Assert.AreEqual(user.Email, returnedUser.Email);
            Assert.AreEqual(user.Password, returnedUser.Password);
        }

        [Test]
        public void ValidateUser_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new RegisterController.ValidateRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            // Setting up the mock to return the correct user but invalid password
            var storedUser = new Register
            {
                Email = "test@example.com",
                Password = "password123"
            };
            _mockRepository.Setup(repo => repo.GetUserByEmail(request.Email)).Returns(storedUser);

            // Act
            var result = _controller.ValidateUser(request) as UnauthorizedObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            // Use reflection to get the value of the "message" property
            var value = result.Value;
            Assert.IsNotNull(value);

            var messageProperty = value.GetType().GetProperty("message");
            Assert.IsNotNull(messageProperty); // Ensure the property exists
            var messageValue = messageProperty.GetValue(value, null) as string; // Get the value of "message"

            Assert.AreEqual("Invalid credentials.", messageValue);
        }


    }
}

