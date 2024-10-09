using AlbumService.Models.AlbumService.Models;
using AlbumService.Models;
using Microsoft.EntityFrameworkCore;
using AlbumService.Exceptions;

namespace AlbumService.Repository
{
    public class ArtistRepository : IArtistRepository
    {

        private readonly AlbumServiceContext _context;

        public ArtistRepository(AlbumServiceContext context)
        {
            _context = context;
        }

        public IQueryable<Artist> GetAllArtistsAsync()
        {
            var artists = _context.Artists;
            if (!artists.Any())
            {
                throw new ArtistNotFoundException("No artists found.");
            }
            return artists;
        }
        public async Task AddArtistAsync(Artist artist)
        {
            var existingArtist = await _context.Artists
                .FirstOrDefaultAsync(a => a.MusicId == artist.MusicId);
            if (existingArtist != null)
            {
                throw new DuplicateAlbumException($"Artist with ID {artist.MusicId} already exists.");
            }
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }
    }

}
