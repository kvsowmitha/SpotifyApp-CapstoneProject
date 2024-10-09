using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbumService.Models;
using AlbumService.Services;

namespace AlbumService.Controllers
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
        public IActionResult GetAllArtists()
        {
            var albums = _artistService.GetAllArtistsAsync();
            return Ok(albums);
        }

        [HttpPost]
        public async Task<ActionResult> AddArtist([FromBody] Artist artist)
        {
            await _artistService.AddArtistAsync(artist);
            return CreatedAtAction(nameof(GetAllArtists), new { id = artist.MusicId }, artist);
        }   
    }
}
