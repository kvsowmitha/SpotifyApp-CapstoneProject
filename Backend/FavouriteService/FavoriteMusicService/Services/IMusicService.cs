using FavoriteMusicService.Models;

namespace FavoriteMusicService.Services
{
    public interface IMusicService
    {
        Task<List<Music>> GetAllMusicAsync();
        Task<Music> AddToFavoritesAsync(string userId, Music music);
        Task<List<Music>> GetUserFavoritesAsync(string userId);
        Task RemoveMusicAsync(string userId, string musicId);
    }
}
