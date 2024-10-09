using AlbumService.Models;

namespace AlbumService.Repository
{
    public interface IArtistRepository
    {       
        IQueryable<Artist> GetAllArtistsAsync();
        
        Task AddArtistAsync(Artist artist);
   
    }
}
