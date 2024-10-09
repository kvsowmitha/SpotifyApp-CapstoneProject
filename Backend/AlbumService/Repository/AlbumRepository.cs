using AlbumService.Models.AlbumService.Models;
using AlbumService.Models;
using Microsoft.EntityFrameworkCore;
using AlbumService.Exceptions;

namespace AlbumService.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AlbumServiceContext _context;

        public AlbumRepository(AlbumServiceContext context)
        {
            _context = context;
        }
        public  IQueryable<Album> GetAllAlbumsAsync()
        {
            var albums = _context.Albums;
            if (!albums.Any())
            {
                throw new AlbumNotFoundException("No albums found.");
            }
            return albums;
        }
        public async Task AddAlbumAsync(Album album)
        {
            var existingAlbum = await _context.Albums
                .FirstOrDefaultAsync(a => a.MusicId == album.MusicId);
            if (existingAlbum != null)
            {
                throw new DuplicateAlbumException($"Album with ID {album.MusicId} already exists.");
            }
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }
    }

}
