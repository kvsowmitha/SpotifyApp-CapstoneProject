using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MusicService.Models
{
    public class Artist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // Ensure MongoDB stores this as an integer
        public string ArtistId { get; set; } // Change to int for MongoDB

        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public int MonthlyListeners { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
