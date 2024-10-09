using FavoriteMusicService.DbContext;
using FavoriteMusicService.Services; // Ensure you use the correct namespace for services
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FavoriteMusicService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Configure MongoDB settings from appsettings.json
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

            builder.Services.AddConsul(builder.Configuration);
            // CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin() // Change this to your Angular app URL
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            // Register MongoDbContext as a Singleton service
            builder.Services.AddSingleton<MusicMongoDbContext>();

            // Register IMusicService to be used by controllers
            builder.Services.AddScoped<IMusicService, MusicService>();

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

            // Enable CORS
            app.UseCors("AllowAnyOrigin");

            app.UseAuthorization();

            app.MapControllers();

            app.UseConsul(app.Configuration);

            app.Run();
        }
    }
}
