using Moq;
using NUnit.Framework;
using AlbumService.Models;
using AlbumService.Repository;
using AlbumService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumService.Tests
{
    [TestFixture]
    public class ArtistServiceTests
    {
        private Mock<IArtistRepository> _mockArtistRepository;
        private ArtistService _artistService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the mock repository and inject it into the ArtistService
            _mockArtistRepository = new Mock<IArtistRepository>();
            _artistService = new ArtistService(_mockArtistRepository.Object);
        }

        [Test]
        public void GetAllArtistsAsync_ReturnsAllArtists()
        {
            // Arrange
            var artists = new List<Artist>
            {
                new Artist { MusicId = "1", MusicName = "Artist 1", SingerName = "Singer 1", PictureUrl = "url1", SongUrl = "url1" },
                new Artist { MusicId = "2", MusicName = "Artist 2", SingerName = "Singer 2", PictureUrl = "url2", SongUrl = "url2" }
            }.AsQueryable();

            _mockArtistRepository.Setup(repo => repo.GetAllArtistsAsync()).Returns(artists);

            // Act
            var result = _artistService.GetAllArtistsAsync().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Artist 1", result[0].MusicName);
            Assert.AreEqual("Artist 2", result[1].MusicName);
        }

        [Test]
        public async Task AddArtistAsync_AddsArtistSuccessfully()
        {
            // Arrange
            var artist = new Artist { MusicId = "3", MusicName = "Artist 3", SingerName = "Singer 3", PictureUrl = "url3", SongUrl = "url3" };

            // Act
            await _artistService.AddArtistAsync(artist);

            // Assert
            _mockArtistRepository.Verify(repo => repo.AddArtistAsync(artist), Times.Once);
        }
    }
}
