using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicService.Models
{
    public class CreateSongRequest
    {
        public string Name { get; set; }
        public string AudioUrl { get; set; }
        public string PictureUrl { get; set; } // Assuming this is a URL
        public string Language { get; set; }
      //  [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public string ArtistName { get; set; }
      //  [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public string AlbumName { get; set; }
        //[BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public string CategoryName { get; set; }
        public TimeSpan Duration { get; set; }
        public string Singers { get; set; }
        public int NumberOfPlays { get; set; }
    }
}
