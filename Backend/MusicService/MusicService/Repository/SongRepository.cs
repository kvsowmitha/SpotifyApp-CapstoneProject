using MongoDB.Bson;
using MongoDB.Driver;
using MusicService.Models;
using System.Xml.Linq;

namespace MusicService.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly MongoDbContext _context;

        public SongRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetSongsByArtistNameAsync(string name)
        {
            var filter = Builders<Song>.Filter.Regex(s => s.ArtistName, new BsonRegularExpression(name, "i"));
            return await _context.Songs.Find(filter).ToListAsync();
            //return await _context.Songs.Find(s => s.ArtistName == name).ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByAlbumNameAsync(string name)
        {
            var filter = Builders<Song>.Filter.Regex(s => s.AlbumName, new BsonRegularExpression(name, "i"));
            return await _context.Songs.Find(filter).ToListAsync();
            //   return await _context.Songs.Find(s => s.AlbumName == name).ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByCategoryNameAsync(string name)
        {
            var filter = Builders<Song>.Filter.Regex(s => s.CategoryName, new BsonRegularExpression(name, "i"));
            return await _context.Songs.Find(filter).ToListAsync();
            //return await _context.Songs.Find(s => s.CategoryName == name).ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByLanguageAsync(string language)
        {
            var filter = Builders<Song>.Filter.Regex(s => s.Language, new BsonRegularExpression(language, "i"));
            return await _context.Songs.Find(filter).ToListAsync();
            //return await _context.Songs.Find(s => s.Language == language).ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _context.Songs.Find(_ => true).ToListAsync();
        }

        public async Task<Song> GetSongByIdAsync(string songId)
        {
            return await _context.Songs.Find(s => s.SongId == songId).FirstOrDefaultAsync();
        }

        public async Task<Song> AddSongAsync(Song song)
        {
            await _context.Songs.InsertOneAsync(song);
            return song;
        }
    }
}