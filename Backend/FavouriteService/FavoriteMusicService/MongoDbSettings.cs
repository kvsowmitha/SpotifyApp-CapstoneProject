namespace FavoriteMusicService
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MusicCollectionName { get; set; }
        public string UserFavoritesCollectionName { get; set; }
    }
}
