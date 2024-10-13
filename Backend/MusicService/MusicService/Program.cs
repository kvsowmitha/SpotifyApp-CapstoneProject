
using MusicService.Models;
using MusicService.Repository;
using MusicService.Services;

namespace MusicService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBSettings"));
            builder.Services.AddSingleton<MongoDbContext>();
            // Add services to the container.
            builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
            builder.Services.AddScoped<ISongRepository, SongRepository>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); // Register CategoryRepository
            builder.Services.AddScoped<ICategoryService, CategoryService>(); // Register CategoryService
            builder.Services.AddScoped<ISongService, SongService>();
            builder.Services.AddControllers();
            builder.Services.AddConsul(builder.Configuration);
            // CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200") // Change this to your Angular app URL
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();


            app.MapControllers();
            app.UseConsul(app.Configuration);
            app.Run();
        }
    }
}
