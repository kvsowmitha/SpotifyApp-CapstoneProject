using FavoriteMusicService.Models;
using FavoriteMusicService.Services;
using Microsoft.AspNetCore.Mvc;
using FavoriteMusicService.Exceptions;

namespace FavoriteMusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllMusic()
        {
            var music = await _musicService.GetAllMusicAsync();
            return Ok(music);
        }

        [HttpPost("favorites/add/{userId}")]
        public async Task<IActionResult> AddToFavorites(string userId, [FromBody] Music music)
        {
            try
            {
                var addedMusic = await _musicService.AddToFavoritesAsync(userId, music);
                return Ok(addedMusic);
            }
            catch (InvalidOperationException ex)
            {
                // If the song already exists, return a conflict status code with the error message
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("favorites/{userId}")]
        public async Task<IActionResult> GetUserFavorites(string userId)
        {
            try
            {
                var favorites = await _musicService.GetUserFavoritesAsync(userId);
                return Ok(favorites);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("favorites/remove/{userId}/{musicId}")]
        public async Task<IActionResult> RemoveFromFavorites(string userId, string musicId)
        {
            try
            {
                await _musicService.RemoveMusicAsync(userId, musicId);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MusicNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
