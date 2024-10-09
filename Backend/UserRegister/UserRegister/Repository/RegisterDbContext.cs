using Microsoft.EntityFrameworkCore;
using UserRegister.Models;

namespace UserRegister.Repository
{
    public class RegisterDbContext : DbContext
    {
        public RegisterDbContext(DbContextOptions<RegisterDbContext> options)
            : base(options)
        {
        }
        public DbSet<Register> Users { get; set; } // Your User entity
        
    }
}


