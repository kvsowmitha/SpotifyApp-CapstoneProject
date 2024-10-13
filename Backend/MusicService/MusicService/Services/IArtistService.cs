using MongoDB.Bson;
using MusicService.Models;

namespace MusicService.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<Artist> GetArtistByIdAsync(string artistId); // Use ObjectId for MongoDB
        Task<Artist> GetArtistByNameAsync(string name); // Use ObjectId for MongoDB

        Task AddArtistAsync(Artist artist);
    }
}
