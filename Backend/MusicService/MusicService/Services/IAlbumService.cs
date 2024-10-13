using MongoDB.Bson;
using MusicService.Models;

namespace MusicService.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album> GetAlbumByIdAsync(string id);  // Change to ObjectId
        Task AddAlbumAsync(Album album);
        Task<Album> GetAlbumByNameAsync(string name);
        Task DeleteAlbumAsync(string id);  // Change to ObjectId
    }
}
