using System.ComponentModel.DataAnnotations;

namespace AlbumService.Models
{
    public class Artist
    {
        [Key]
        public string MusicId { get; set; }
        public string MusicName { get; set; }
        public string SingerName { get; set; }
        public string PictureUrl { get; set; }
        public string SongUrl { get; set; }
    }


}
