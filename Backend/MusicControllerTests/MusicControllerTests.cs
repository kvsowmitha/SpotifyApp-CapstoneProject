using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicService.Models;
using MusicService.Services; // Assuming your services are in this namespace

namespace MusicService.Tests
{
    [TestFixture]
    public class MusicServiceTests
    {
        private Mock<IAlbumService> _albumServiceMock;
        private Mock<IArtistService> _artistServiceMock;
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<ISongService> _songServiceMock;

        [SetUp]
        public void SetUp()
        {
            _albumServiceMock = new Mock<IAlbumService>();
            _artistServiceMock = new Mock<IArtistService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _songServiceMock = new Mock<ISongService>();
        }

        #region Album Tests

        [Test]
        public async Task GetAlbumById_ShouldReturnAlbum_WhenIdExists()
        {
            // Arrange
            var albumId = "60c72b2f9f1b9b2d88bfaefa";
            var expectedAlbum = new Album
            {
                AlbumId = albumId,
                Name = "Test Album",
                MusicDirector = "Test Director",
                NoOfTracks = 10,
                ReleaseYear = 2020
            };
            _albumServiceMock.Setup(a => a.GetAlbumByIdAsync(albumId)).ReturnsAsync(expectedAlbum);

            // Act
            var result = await _albumServiceMock.Object.GetAlbumByIdAsync(albumId);

            // Assert
            Assert.AreEqual(expectedAlbum, result);
        }

        [Test]
        public async Task AddAlbum_ShouldAddAlbumSuccessfully()
        {
            // Arrange
            var newAlbum = new Album
            {
                Name = "New Album",
                MusicDirector = "New Director",
                NoOfTracks = 8,
                ReleaseYear = 2021
            };
            _albumServiceMock.Setup(a => a.AddAlbumAsync(newAlbum)).Returns(Task.CompletedTask);

            // Act
            await _albumServiceMock.Object.AddAlbumAsync(newAlbum);

            // Assert
            _albumServiceMock.Verify(a => a.AddAlbumAsync(newAlbum), Times.Once);
        }

        #endregion

        #region Artist Tests

        [Test]
        public async Task GetArtistById_ShouldReturnArtist_WhenIdExists()
        {
            // Arrange
            var artistId = "60c72b2f9f1b9b2d88bfaefb";
            var expectedArtist = new Artist
            {
                ArtistId = artistId,
                Name = "Test Artist",
                MonthlyListeners = 50000
            };
            _artistServiceMock.Setup(a => a.GetArtistByIdAsync(artistId)).ReturnsAsync(expectedArtist);

            // Act
            var result = await _artistServiceMock.Object.GetArtistByIdAsync(artistId);

            // Assert
            Assert.AreEqual(expectedArtist, result);
        }

        [Test]
        public async Task AddArtist_ShouldAddArtistSuccessfully()
        {
            // Arrange
            var newArtist = new Artist
            {
                Name = "New Artist",
                MonthlyListeners = 100000
            };
            _artistServiceMock.Setup(a => a.AddArtistAsync(newArtist)).Returns(Task.CompletedTask);

            // Act
            await _artistServiceMock.Object.AddArtistAsync(newArtist);

            // Assert
            _artistServiceMock.Verify(a => a.AddArtistAsync(newArtist), Times.Once);
        }

        [Test]
        public async Task AddCategory_ShouldAddCategorySuccessfully()
        {
            // Arrange
            var newCategory = new Category
            {
                Name = "New Category"
            };
            _categoryServiceMock.Setup(c => c.AddCategoryAsync(newCategory)).Returns(Task.CompletedTask);

            // Act
            await _categoryServiceMock.Object.AddCategoryAsync(newCategory);

            // Assert
            _categoryServiceMock.Verify(c => c.AddCategoryAsync(newCategory), Times.Once);
        }

        #endregion

        #region Song Tests

        [Test]
        public async Task GetSongById_ShouldReturnSong_WhenIdExists()
        {
            // Arrange
            var songId = "60c72b2f9f1b9b2d88bfaefd";
            var expectedSong = new Song
            {
                SongId = songId,
                Name = "Test Song",
                ArtistName = "Test Artist",
                AlbumName = "Test Album",
                CategoryName = "Test Category",
                Duration = new System.TimeSpan(0, 3, 45),
                NumberOfPlays = 100000
            };
            _songServiceMock.Setup(s => s.GetSongByIdAsync(songId)).ReturnsAsync(expectedSong);

            // Act
            var result = await _songServiceMock.Object.GetSongByIdAsync(songId);

            // Assert
            Assert.AreEqual(expectedSong, result);
        }

        [Test]
        public async Task AddSong_ShouldAddSongSuccessfully()
        {
            // Arrange
            var newSong = new Song
            {
                Name = "New Song",
                ArtistName = "New Artist",
                AlbumName = "New Album",
                CategoryName = "New Category",
                Duration = new System.TimeSpan(0, 4, 10)
            };
            _songServiceMock.Setup(s => s.AddSongAsync(newSong)).Returns(Task.CompletedTask);

            // Act
            await _songServiceMock.Object.AddSongAsync(newSong);

            // Assert
            _songServiceMock.Verify(s => s.AddSongAsync(newSong), Times.Once);
        }

        #endregion
    }
}
