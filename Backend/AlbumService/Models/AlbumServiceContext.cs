namespace AlbumService.Models
{
    using Microsoft.EntityFrameworkCore;

    namespace AlbumService.Models
    {
        public class AlbumServiceContext : DbContext
        {
            public AlbumServiceContext(DbContextOptions<AlbumServiceContext> options)
                : base(options)
            {
            }
       
            public DbSet<Album> Albums { get; set; }
            public DbSet<Artist> Artists { get; set; }



        }
    }

}
