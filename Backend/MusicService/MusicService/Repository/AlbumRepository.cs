using MongoDB.Bson;
using MongoDB.Driver;
using MusicService.Models;
using System.Text.RegularExpressions;

namespace MusicService.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MongoDbContext _context;

        public AlbumRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _context.Albums.Find(_ => true).ToListAsync(); // Fetch all albums
        }

        public async Task<Album> GetAlbumByIdAsync(string id)
        {
            return await _context.Albums.Find(a => a.AlbumId == id).FirstOrDefaultAsync(); // Find by ObjectId
        }

        public async Task AddAlbumAsync(Album album)
        {
            await _context.Albums.InsertOneAsync(album); // Insert album into MongoDB
        }

        public async Task<Album> GetAlbumByNameAsync(string name)
        {
            //return await _context.Albums.Find(a => a.Name == name).FirstOrDefaultAsync();
            var filter = Builders<Album>.Filter.Regex(a => a.Name, new BsonRegularExpression(name, "i"));
            return await _context.Albums.Find(filter).FirstOrDefaultAsync();
        }
        public async Task DeleteAlbumAsync(string id)
        {
            await _context.Albums.DeleteOneAsync(a => a.AlbumId == id); // Delete by ObjectId
        }
    }
}
