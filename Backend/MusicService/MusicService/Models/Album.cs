using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MusicService.Models
{
    public class Album
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  // Use MongoDB ObjectId for automatic id generation
        public string AlbumId { get; set; } // Change to int for MongoDB

        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string MusicDirector { get; set; }
        public int NoOfTracks { get; set; }
        public int ReleaseYear { get; set; }


        // Navigation properties (optional, could be embedded)
        public ICollection<Song> Songs { get; set; }
    }
}
