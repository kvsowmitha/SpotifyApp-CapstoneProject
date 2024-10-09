namespace FavoriteMusicService.Exceptions
{
    public class MusicNotFoundException : Exception
    {
        public MusicNotFoundException() : base("Music not found.") { }

        public MusicNotFoundException(string message) : base(message) { }

        public MusicNotFoundException(string message, Exception inner) : base(message, inner) { }

    }
}
