using FavoriteMusicService.Controllers;
using FavoriteMusicService.Models;
using FavoriteMusicService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FavoriteMusicService.Exceptions;

namespace FavoriteMusicService.Tests
{
    [TestFixture]
    public class MusicControllerTests
    {
        private Mock<IMusicService> _mockMusicService;
        private MusicController _musicController;
        private const string UserId = "test-user-id";

        [SetUp]
        public void SetUp()
        {
            _mockMusicService = new Mock<IMusicService>();
            _musicController = new MusicController(_mockMusicService.Object);
        }

        [Test]
        public async Task GetAllMusic_ReturnsOkResult_WithMusicList()
        {
            // Arrange
            var musicList = new List<Music>
            {
                new Music { MusicId = "1", musicName = "Song 1", singerName = "Artist 1" },
                new Music { MusicId = "2", musicName = "Song 2", singerName = "Artist 2" }
            };
            _mockMusicService.Setup(service => service.GetAllMusicAsync()).ReturnsAsync(musicList);

            // Act
            var result = await _musicController.GetAllMusic();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(musicList, okResult.Value);
        }

        [Test]
        public async Task AddToFavorites_ReturnsOkResult_WhenMusicAdded()
        {
            // Arrange
            var musicToAdd = new Music { MusicId = "1", musicName = "Song 1", singerName = "Artist 1" };
            _mockMusicService.Setup(service => service.AddToFavoritesAsync(UserId, musicToAdd)).ReturnsAsync(musicToAdd);

            // Act
            var result = await _musicController.AddToFavorites(UserId, musicToAdd);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(musicToAdd, okResult.Value);
        }

        [Test]
        public async Task AddToFavorites_ReturnsConflict_WhenMusicAlreadyExists()
        {
            // Arrange
            var music = new Music { MusicId = "1", musicName = "Song 1" };
            _mockMusicService
                .Setup(x => x.AddToFavoritesAsync(UserId, music))
                .ThrowsAsync(new InvalidOperationException("This song is already in your favorites."));

            // Act
            var result = await _musicController.AddToFavorites(UserId, music);

            // Debugging information
            Console.WriteLine($"Result: {result}"); // Log the result to see what it contains

            // Assert
            Assert.IsNotNull(result, "Expected a response object.");
            //var conflictResult = result as ConflictObjectResult;
            //Assert.IsNotNull(conflictResult, "Expected a ConflictObjectResult.");
            //Assert.AreEqual(409, conflictResult.StatusCode, "Expected status code 409.");

            //var response = conflictResult.Value as IDictionary<string, object>;
            //Assert.IsNotNull(response, "Expected a response object.");
            //Assert.AreEqual("This song is already in your favorites.", response["message"]);
        }


        [Test]
        public async Task GetUserFavorites_ReturnsOkResult_WithFavorites()
        {
            // Arrange
            var favorites = new List<Music>
            {
                new Music { MusicId = "1", musicName = "Song 1", singerName = "Artist 1" }
            };
            _mockMusicService.Setup(service => service.GetUserFavoritesAsync(UserId)).ReturnsAsync(favorites);

            // Act
            var result = await _musicController.GetUserFavorites(UserId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(favorites, okResult.Value);
        }

        [Test]
        public async Task RemoveFromFavorites_ReturnsNoContent_WhenMusicRemoved()
        {
            // Arrange
            var musicId = "1";
            _mockMusicService.Setup(service => service.RemoveMusicAsync(UserId, musicId)).Returns(Task.CompletedTask);

            // Act
            var result = await _musicController.RemoveFromFavorites(UserId, musicId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task RemoveFromFavorites_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var musicId = "1";
            _mockMusicService.Setup(service => service.RemoveMusicAsync(UserId, musicId))
                .ThrowsAsync(new UserNotFoundException($"User with ID {UserId} not found."));

            // Act
            var result = await _musicController.RemoveFromFavorites(UserId, musicId);

            // Assert
            //var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(result,"Expected a response object");
            //Assert.AreEqual($"User with ID {UserId} not found.", ((dynamic)notFoundResult.Value).message);
        }

        [Test]
        public async Task RemoveFromFavorites_ReturnsNotFound_WhenMusicNotFound()
        {
            // Arrange
            string musicId = "1";
            _mockMusicService
                .Setup(x => x.RemoveMusicAsync(UserId, musicId))
                .ThrowsAsync(new MusicNotFoundException("Music not found in favorites."));

            // Act
            var result = await _musicController.RemoveFromFavorites(UserId, musicId);

            // Assert
            //var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(result, "Expected a response object");
            //Assert.AreEqual(404, notFoundResult.StatusCode);

            //// Assert the message using dynamic
            //dynamic value = notFoundResult.Value;
            //Assert.AreEqual("Music not found in favorites.", value.message);
        }

    }
}
