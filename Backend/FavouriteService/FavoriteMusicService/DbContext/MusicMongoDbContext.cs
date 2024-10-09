using FavoriteMusicService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FavoriteMusicService.DbContext
{
    public class MusicMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MusicMongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        // Access to Music collection
        public IMongoCollection<Music> MusicCollection =>
            _database.GetCollection<Music>(nameof(Music));

        // Access to UserFavorites collection
        public IMongoCollection<UserFavorite> UserFavoritesCollection =>
            _database.GetCollection<UserFavorite>(nameof(UserFavorite));
    }
}
