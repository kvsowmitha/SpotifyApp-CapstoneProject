using AlbumService.Models;
using System.Linq;

namespace AlbumService.Repository
{
    public interface IAlbumRepository
    {
        IQueryable<Album> GetAllAlbumsAsync();
        Task AddAlbumAsync(Album album);
    }
}
