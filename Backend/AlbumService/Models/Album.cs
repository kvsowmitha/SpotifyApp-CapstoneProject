using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AlbumService.Models
{
    public class Album
    {
        [Key]
        public string MusicId { get; set; }
        public string MusicName { get; set; }
        public string SingerName { get; set; }
        public string PictureUrl { get; set; }
        public string SongUrl { get; set; }

        
    }
}
