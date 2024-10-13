using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAllAlbums()
        {
            try
            {
                var albums = await _albumService.GetAllAlbumsAsync();
                return Ok(albums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving albums: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetAlbumById(string id) // Change to int
        {
            try
            {
                var album = await _albumService.GetAlbumByIdAsync(id);
                return Ok(album);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<Album>> GetAlbumByName(string name)
        {
            try
            {
                var album = await _albumService.GetAlbumByNameAsync(name);
                if (album == null)
                {
                    return NotFound(new { message = $"Album with name '{name}' not found." });
                }
                return Ok(album);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddAlbum([FromBody] AlbumCreateDto albumDto)
        {
            try
            {
                var album = new Album
                {
                    Name = albumDto.Name,
                    PictureUrl = albumDto.PictureUrl,
                    MusicDirector = albumDto.MusicDirector,
                    NoOfTracks = albumDto.NoOfTracks,
                    ReleaseYear = albumDto.ReleaseYear
                };

                await _albumService.AddAlbumAsync(album);
                return CreatedAtAction(nameof(GetAlbumById), new { id = album.AlbumId }, album);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlbum(string id) // Change to int
        {
            try
            {
                var album = await _albumService.GetAlbumByIdAsync(id); // Pass int ID
                if (album == null)
                {
                    return NotFound(new { message = $"Album with ID {id} not found." });
                }

                await _albumService.DeleteAlbumAsync(id); // Pass int ID
                return Ok(new { message = $"Album with ID {id} has been successfully deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while deleting the album: {ex.Message}" });
            }
        }
    }
}
