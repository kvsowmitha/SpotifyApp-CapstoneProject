using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MusicService.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // Use MongoDB ObjectId for automatic id generation
        public string CategoryId { get; set; }    // Change from int to string to store ObjectId as string
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
