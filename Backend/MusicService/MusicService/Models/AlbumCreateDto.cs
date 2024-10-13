using MongoDB.Bson.Serialization.Attributes;

namespace MusicService.Models
{
    public class AlbumCreateDto
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }  // URL to the picture as string
        public string MusicDirector { get; set; }
        public int NoOfTracks { get; set; }
        public int ReleaseYear { get; set; }

      //  [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
       // public int ArtistId { get; set; }  // String representation of ObjectId
    }
}
