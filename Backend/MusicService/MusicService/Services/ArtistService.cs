using MongoDB.Bson;
using MusicService.Models;
using MusicService.Repository;

namespace MusicService.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _artistRepository.GetAllArtistsAsync();
        }

        public async Task<Artist> GetArtistByIdAsync(string artistId) // Use ObjectId for MongoDB
        {
            return await _artistRepository.GetArtistByIdAsync(artistId);
        }
        public async Task<Artist> GetArtistByNameAsync(string name) // Use ObjectId for MongoDB
        {
            return await _artistRepository.GetArtistByNameAsync(name);
        }

        public async Task AddArtistAsync(Artist artist)
        {
            await _artistRepository.AddArtistAsync(artist);
        }
    }
}