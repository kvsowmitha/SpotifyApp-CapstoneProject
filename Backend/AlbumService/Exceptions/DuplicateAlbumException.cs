namespace AlbumService.Exceptions
{
    public class DuplicateAlbumException : Exception
    {
        public DuplicateAlbumException(string message) : base(message)
        {
        }
    }
}
