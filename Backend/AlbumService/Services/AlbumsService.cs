using AlbumService.Models;
using AlbumService.Repository;

namespace AlbumService.Services
{
    public class AlbumsService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumsService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public  IQueryable<Album> GetAllAlbumsAsync()
        {
            return  _albumRepository.GetAllAlbumsAsync();
        }

        public async Task AddAlbumAsync(Album album)
        {
            await _albumRepository.AddAlbumAsync(album);
        }
    }

}
