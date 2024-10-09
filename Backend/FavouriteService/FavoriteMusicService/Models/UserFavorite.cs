using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FavoriteMusicService.Models
{
    public class UserFavorite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<Music> FavoriteMusic { get; set; }
    }
}
