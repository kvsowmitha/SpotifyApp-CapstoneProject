using AlbumService.Models;
using AlbumService.Repository;

namespace AlbumService.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }


        public IQueryable<Artist> GetAllArtistsAsync()
        {
            return _artistRepository.GetAllArtistsAsync();
        }

        public async Task AddArtistAsync(Artist artist)
        {
            await _artistRepository.AddArtistAsync(artist);
        }

    }

}
