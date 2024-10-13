using MongoDB.Bson;
using MusicService.Models;

namespace MusicService.Repository
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album> GetAlbumByIdAsync(string id);  // Change to ObjectId
        Task AddAlbumAsync(Album album);
        Task<Album> GetAlbumByNameAsync(string name);  // Change to ObjectId

        Task DeleteAlbumAsync(string id);  // Change to ObjectId
    }
}
