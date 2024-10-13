using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MusicService.Models
{
    public class Song
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Ensure MongoDB stores this as an integer
        public string SongId { get; set; } // Change to int for MongoDB
        public string Name { get; set; }
        public string audioUrl { get; set; }
        public string PictureUrl { get; set; }
        public string Language { get; set; }
        public string ArtistName { get; set; } // Use int instead of ObjectId

        public string AlbumName { get; set; } // Use int instead of ObjectId

        public string CategoryName { get; set; } // Use int instead of ObjectId


        // New fields
        public TimeSpan Duration { get; set; }
        public string Singers { get; set; }
        public int NumberOfPlays { get; set; }

        // Navigation properties (optional, could be embedded)
        [JsonIgnore]
        public Artist Artist { get; set; }

        [JsonIgnore]
        public Album Album { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
