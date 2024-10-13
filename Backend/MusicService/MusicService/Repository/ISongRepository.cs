using MongoDB.Bson;
using MusicService.Models;

namespace MusicService.Repository
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetSongsByArtistNameAsync(string name);
        Task<IEnumerable<Song>> GetSongsByAlbumNameAsync(string name);
        Task<IEnumerable<Song>> GetSongsByCategoryNameAsync(string name);
        Task<IEnumerable<Song>> GetSongsByLanguageAsync(string language);
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song> GetSongByIdAsync(string songId);
        Task<Song> AddSongAsync(Song song);
    }
}
