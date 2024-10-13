using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MusicService.Models
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Album> Albums => _database.GetCollection<Album>("Albums");
        public IMongoCollection<Artist> Artists => _database.GetCollection<Artist>("Artists");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Song> Songs => _database.GetCollection<Song>("Songs");
    }
}
