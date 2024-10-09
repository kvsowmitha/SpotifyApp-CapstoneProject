using AlbumService.Models;

namespace AlbumService.Services
{
    public interface IAlbumService
    {
        IQueryable<Album> GetAllAlbumsAsync();
        Task AddAlbumAsync(Album album);
    }
}
