using Moq;                   // For Mocking objects
using NUnit.Framework;        // For NUnit Testing
using System.Linq;            // For LINQ methods like ToList()
using System.Threading.Tasks; // For async methods
using AlbumService.Models;    // For accessing Album model
using AlbumService.Repository; // For accessing IAlbumRepository
using AlbumService.Services;  // For accessing AlbumsService

namespace AlbumService.Tests
{
    [TestFixture]
    public class AlbumsServiceTests
    {
        private Mock<IAlbumRepository> _mockAlbumRepository;
        private AlbumsService _albumsService;

        [SetUp]
        public void SetUp()
        {
            _mockAlbumRepository = new Mock<IAlbumRepository>();
            _albumsService = new AlbumsService(_mockAlbumRepository.Object);
        }

        [Test]
        public void GetAllAlbumsAsync_ReturnsAllAlbums()
        {
            // Arrange
            var albums = new List<Album>
            {
                new Album { MusicId = "1", MusicName = "Album 1", SingerName = "Singer 1", PictureUrl = "url1", SongUrl = "url1" },
                new Album { MusicId = "2", MusicName = "Album 2", SingerName = "Singer 2", PictureUrl = "url2", SongUrl = "url2" }
            }.AsQueryable();

            _mockAlbumRepository.Setup(repo => repo.GetAllAlbumsAsync()).Returns(albums);

            // Act
            var result = _albumsService.GetAllAlbumsAsync().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Album 1", result[0].MusicName);
            Assert.AreEqual("Album 2", result[1].MusicName);
        }

        [Test]
        public async Task AddAlbumAsync_AddsAlbumSuccessfully()
        {
            // Arrange
            var album = new Album { MusicId = "3", MusicName = "Album 3", SingerName = "Singer 3", PictureUrl = "url3", SongUrl = "url3" };

            // Act
            await _albumsService.AddAlbumAsync(album);

            // Assert
            _mockAlbumRepository.Verify(repo => repo.AddAlbumAsync(album), Times.Once);
        }
    }
}
