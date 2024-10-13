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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                if (categories == null || !categories.Any())
                {
                    return Ok(new { Message = "No categories available at the moment." });
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] CategoryCreateDto categoryDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryDto.Name))
                {
                    return BadRequest("Category name is required.");
                }

                var category = new Category
                {
                    Name = categoryDto.Name,
                    PictureUrl = categoryDto.PictureUrl  // Use PictureUrl as a string URL
                };

                await _categoryService.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetAllCategories), new { id = category.CategoryId }, category); // Use int id directly
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
