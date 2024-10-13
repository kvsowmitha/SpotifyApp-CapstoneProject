using MongoDB.Bson;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Repository;

namespace MusicService.Services
{
    public class AlbumService: IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

    public AlbumService(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
    {
        return await _albumRepository.GetAllAlbumsAsync();
    }

    public async Task<Album> GetAlbumByIdAsync(string id)  // Change to ObjectId
    {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null)
            {
                throw new NotFoundException($"Album with ID {id} not found.");
            }
            return album;
        }

    public async Task AddAlbumAsync(Album album)
    {
            var existingAlbum = await _albumRepository.GetAlbumByNameAsync(album.Name);
            if (existingAlbum != null)
            {
                throw new ConflictException($"Album with name '{album.Name}' already exists.");
            }
            await _albumRepository.AddAlbumAsync(album);
        }

     public async Task<Album> GetAlbumByNameAsync(string name) // Corrected return type
        {
            return await _albumRepository.GetAlbumByNameAsync(name);
        }
    public async Task DeleteAlbumAsync(string id)  // Change to ObjectId
    {
        await _albumRepository.DeleteAlbumAsync(id);
    }
}
}
