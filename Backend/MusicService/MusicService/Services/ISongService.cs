using MongoDB.Bson;
using MusicService.Models;

namespace MusicService.Services
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetSongsByArtistNameAsync(string name);
        Task<IEnumerable<Song>> GetSongsByAlbumNameAsync(string name);
        Task<IEnumerable<Song>> GetSongsByCategoryNameAsync(string name);  // Use ObjectId for MongoDB
        Task<IEnumerable<Song>> GetSongsByLanguageAsync(string language);
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song> GetSongByIdAsync(string songId);  // Use ObjectId for MongoDB

        Task AddSongAsync(Song song);
    }
}
