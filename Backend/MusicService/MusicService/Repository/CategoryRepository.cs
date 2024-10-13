using MongoDB.Driver;
using MusicService.Models;

namespace MusicService.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MongoDbContext _context;

        public CategoryRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Find(_ => true).ToListAsync(); // Fetch all categories
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.InsertOneAsync(category); // Insert category into MongoDB
        }
    }
}
