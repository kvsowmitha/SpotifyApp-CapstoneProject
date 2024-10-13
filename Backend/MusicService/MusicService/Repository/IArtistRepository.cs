using MongoDB.Bson;
using MusicService.Models;

namespace MusicService.Repository
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<Artist> GetArtistByIdAsync(string artistId); // Change to ObjectId
        Task<Artist> GetArtistByNameAsync(string name);
        Task AddArtistAsync(Artist artist);
    }
}
