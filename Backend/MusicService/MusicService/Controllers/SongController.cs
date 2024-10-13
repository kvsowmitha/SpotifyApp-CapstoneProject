using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using MusicService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService; // Assuming ISongService is defined correctly

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet("by-artist/{artistName}")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongsByArtist(string artistName)
        {
            try
            {
                var songs = await _songService.GetSongsByArtistNameAsync(artistName);
                if (songs == null || !songs.Any())
                {
                    return NotFound("No songs found for the given artist.");
                }
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-album/{albumName}")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongsByAlbum(string albumName)
        {
            try
            {
                var songs = await _songService.GetSongsByAlbumNameAsync(albumName);
                if (songs == null || !songs.Any())
                {
                    return NotFound("No songs found for the given album.");
                }
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-category/{categoryName}")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongsByCategory(string categoryName)
        {
            try
            {
                var songs = await _songService.GetSongsByCategoryNameAsync(categoryName);
                if (songs == null || !songs.Any())
                {
                    return NotFound("No songs found for the given category.");
                }
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-language/{language}")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongsByLanguage(string language)
        {
            try
            {
                var songs = await _songService.GetSongsByLanguageAsync(language);
                if (songs == null || !songs.Any())
                {
                    return NotFound("No songs found for the given language.");
                }
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetAllSongs()
        {
            try
            {
                var songs = await _songService.GetAllSongsAsync();
                if (songs == null || !songs.Any())
                {
                    return NoContent();
                }
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{songId}")]
        public async Task<ActionResult<Song>> GetSongById(string songId)
        {
            try
            {
                var song = await _songService.GetSongByIdAsync(songId);
                if (song == null)
                {
                    return NotFound($"Song with ID {songId} not found.");
                }
                return Ok(song);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddSong([FromBody] CreateSongRequest songRequest) // Adjust based on your CreateSongRequest model
        {
            try
            {
                // Check if audio and image URLs are provided
                if (string.IsNullOrWhiteSpace(songRequest.AudioUrl) || string.IsNullOrWhiteSpace(songRequest.PictureUrl))
                {
                    return BadRequest("Audio URL and picture URL are required.");
                }

                // Create Song object
                var song = new Song
                {
                    Name = songRequest.Name,
                    audioUrl = songRequest.AudioUrl, // Store audio URL as string
                    PictureUrl = songRequest.PictureUrl, // Store image URL as string
                    Language = songRequest.Language,
                    ArtistName = songRequest.ArtistName,
                    AlbumName = songRequest.AlbumName,
                    CategoryName = songRequest.CategoryName,
                    Duration = songRequest.Duration,
                    Singers = songRequest.Singers,
                    NumberOfPlays = songRequest.NumberOfPlays
                };

                await _songService.AddSongAsync(song); // Assuming AddSongAsync will handle adding a Song
                return CreatedAtAction(nameof(GetSongById), new { songId = song.SongId }, song); // song.SongId should be an int
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
