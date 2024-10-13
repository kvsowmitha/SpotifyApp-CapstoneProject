namespace MusicService.Models
{
    public class ArtistCreateDto
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }  // URL of the picture
        public int MonthlyListeners { get; set; }
    }
}
