using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbumService.Models;
using AlbumService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AlbumService.Controllers
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
        public IActionResult GetAllAlbums()
        {
            var albums = _albumService.GetAllAlbumsAsync();
            return Ok(albums);
        }

        [HttpPost]
        public async Task<ActionResult> AddAlbum([FromBody] Album album)
        {
            await _albumService.AddAlbumAsync(album);
            return CreatedAtAction(nameof(GetAllAlbums), new { id = album.MusicId }, album);
        }
    }
}
