using AlbumService.Models;

namespace AlbumService.Services
{
    public interface IArtistService
    {
     
        IQueryable<Artist> GetAllArtistsAsync();
        Task AddArtistAsync(Artist artist);
      
    }
}
