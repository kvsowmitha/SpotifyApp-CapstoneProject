namespace AlbumService.Exceptions
{
    public class ArtistNotFoundException : Exception
    {
        public ArtistNotFoundException(string message) : base(message)
        {
        }
    }
}
