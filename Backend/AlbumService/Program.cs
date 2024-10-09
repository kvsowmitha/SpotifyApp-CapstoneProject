using AlbumService.Middleware;
using AlbumService.Models;
using AlbumService.Models.AlbumService.Models;
using AlbumService.Repository;
using AlbumService.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AlbumService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            builder.Services.AddConsul(builder.Configuration);
            // CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin() // Change this to your Angular app URL
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AlbumServiceContext>(options =>
            {
                var conStr = builder.Configuration.GetConnectionString("conStr");
                options.UseSqlServer(conStr);
            });
            // Configure the dependency injection for all the components
            builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
            builder.Services.AddScoped<IAlbumService, AlbumsService>();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // Enable CORS
            app.UseCors("AllowAnyOrigin");

            app.UseMiddleware<ExceptionHandlingMiddleware>();


            app.UseAuthorization();

            app.MapControllers();
            app.UseConsul(app.Configuration);
            app.Run();
        }
    }
}
