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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetAllArtists()
        {
            try
            {
                var artists = await _artistService.GetAllArtistsAsync();
                if (artists == null || !artists.Any())
                {
                    return Ok(new { Message = "No artists available at the moment." });
                }
                return Ok(artists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtistById(string id)
        {
            try
            {
                var artist = await _artistService.GetArtistByIdAsync(id);
                if (artist == null)
                {
                    return NotFound(new { message = $"Artist with ID {id} not found." });
                }
                return Ok(artist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<Artist>> GetArtistByName(string name)
        {
            try
            {
                var artist = await _artistService.GetArtistByNameAsync(name);
                if (artist == null)
                {
                    return NotFound(new { message = $"Artist with name '{name}' not found." });
                }
                return Ok(artist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddArtist([FromBody] ArtistCreateDto artistDto)
        {
            try
            {
                var artist = new Artist
                {
                    Name = artistDto.Name,
                    PictureUrl = artistDto.PictureUrl, // URL to the picture as a string
                    MonthlyListeners = artistDto.MonthlyListeners
                };

                await _artistService.AddArtistAsync(artist);
                return CreatedAtAction(nameof(GetArtistById), new { id = artist.ArtistId }, artist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
