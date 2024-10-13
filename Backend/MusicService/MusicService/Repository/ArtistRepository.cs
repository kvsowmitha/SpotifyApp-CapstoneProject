using MongoDB.Bson;
using MongoDB.Driver;
using MusicService.Models;

namespace MusicService.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MongoDbContext _context;

        public ArtistRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _context.Artists.Find(_ => true).ToListAsync(); // Fetch all artists
        }

        public async Task<Artist> GetArtistByIdAsync(string artistId) // Change to ObjectId
        {
            return await _context.Artists.Find(a => a.ArtistId == artistId).FirstOrDefaultAsync(); // Find by ObjectId
        }
        public async Task<Artist> GetArtistByNameAsync(string name) // Change to ObjectId
        {
            var filter = Builders<Artist>.Filter.Regex(a => a.Name, new BsonRegularExpression(name, "i"));
            return await _context.Artists.Find(filter).FirstOrDefaultAsync();
           // return await _context.Artists.Find(a => a.ArtistId == artistId).FirstOrDefaultAsync(); // Find by ObjectId
        }

        public async Task AddArtistAsync(Artist artist)
        {
            await _context.Artists.InsertOneAsync(artist); // Insert artist into MongoDB
        }
    }
}
