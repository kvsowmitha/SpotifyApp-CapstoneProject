using MongoDB.Bson;
using MusicService.Models;
using MusicService.Repository;

namespace MusicService.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<IEnumerable<Song>> GetSongsByArtistNameAsync(string name)
        {
            return await _songRepository.GetSongsByArtistNameAsync(name);
        }

        public async Task<IEnumerable<Song>> GetSongsByAlbumNameAsync(string name)
        {
            return await _songRepository.GetSongsByAlbumNameAsync(name);
        }

        public async Task<IEnumerable<Song>> GetSongsByCategoryNameAsync(string name)
        {
            return await _songRepository.GetSongsByCategoryNameAsync(name);
        }

        public async Task<IEnumerable<Song>> GetSongsByLanguageAsync(string language)
        {
            return await _songRepository.GetSongsByLanguageAsync(language);
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _songRepository.GetAllSongsAsync();
        }

        public async Task<Song> GetSongByIdAsync(string songId)
        {
            return await _songRepository.GetSongByIdAsync(songId);
        }

        public async Task AddSongAsync(Song song)
        {
            await _songRepository.AddSongAsync(song);
        }
    }
}