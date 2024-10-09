using FavoriteMusicService.DbContext;
using FavoriteMusicService.Exceptions;
using FavoriteMusicService.Models;
using MongoDB.Driver;

namespace FavoriteMusicService.Services
{
    public class MusicService : IMusicService
    {
        private readonly MusicMongoDbContext _context;

        public MusicService(MusicMongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Music>> GetAllMusicAsync()
        {
            return await _context.MusicCollection.Find(m => true).ToListAsync();
        }

        public async Task<Music> AddToFavoritesAsync(string userId, Music music)
        {
            // Find the user's favorites
            var userFavorites = await _context.UserFavoritesCollection
                .Find(fav => fav.UserId == userId)
                .FirstOrDefaultAsync();

            if (userFavorites == null)
            {
                // If no favorites exist for the user, create a new one
                userFavorites = new UserFavorite
                {
                    UserId = userId,
                    FavoriteMusic = new List<Music> { music }
                };
                await _context.UserFavoritesCollection.InsertOneAsync(userFavorites);
            }
            else
            {
                // Check if the song already exists in the user's favorites
                bool musicExists = userFavorites.FavoriteMusic.Any(m => m.MusicId == music.MusicId);

                if (musicExists)
                {
                    throw new InvalidOperationException("This song is already in your favorites.");
                }

                // Add the song to the favorites
                userFavorites.FavoriteMusic.Add(music);
                await _context.UserFavoritesCollection.ReplaceOneAsync(fav => fav.UserId == userId, userFavorites);
            }

            return music;
        }

        public async Task<List<Music>> GetUserFavoritesAsync(string userId)
        {
            var userFavorites = await _context.UserFavoritesCollection.Find(fav => fav.UserId == userId).FirstOrDefaultAsync();

            if (userFavorites == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found.");
            }

            return userFavorites.FavoriteMusic;
        }

        public async Task RemoveMusicAsync(string userId, string musicId)
        {
            var userFavorites = await _context.UserFavoritesCollection.Find(fav => fav.UserId == userId).FirstOrDefaultAsync();

            if (userFavorites == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found.");
            }

            var musicToRemove = userFavorites.FavoriteMusic.FirstOrDefault(m => m.MusicId == musicId);
            if (musicToRemove == null)
            {
                throw new MusicNotFoundException($"Music with ID {musicId} not found in the user's favorites.");
            }

            userFavorites.FavoriteMusic.RemoveAll(m => m.MusicId == musicId);
            await _context.UserFavoritesCollection.ReplaceOneAsync(fav => fav.UserId == userId, userFavorites);
        }
    }
}
