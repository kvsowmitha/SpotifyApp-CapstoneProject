using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Model
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<LoginData> LoginData { get; set; }
    }
}
