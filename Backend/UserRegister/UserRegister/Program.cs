using Microsoft.EntityFrameworkCore;
using UserRegister.Repository;
using UserRegister.Services;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;

namespace UserRegister
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context with SQL Server
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<RegisterDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));

            // Add services to the container
            builder.Services.AddControllers();

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });


            // Register the repository and service
            builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
            builder.Services.AddScoped<IUserService, UserServices>();

            // Configure Kafka settings
            builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
            builder.Services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));
            builder.Services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));
            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User Registration API",
                    Version = "v1",
                    Description = "API for user registration and management"
                });
            });

            builder.Services.AddConsul(builder.Configuration);
            // Configure CORS to allow requests from the Angular app
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    policy => policy.WithOrigins("http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            var app = builder.Build();

            // Enable CORS globally
            app.UseCors("AllowAngularApp");

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Registration API V1");
                });
            }

            app.UseAuthorization();
            app.MapControllers();
            app.UseConsul(app.Configuration);
            app.Run();
        }
    }

    public class KafkaSettings
    {
        public string? BootstrapServers { get; set; }
        public string? TopicName { get; set; }
    }
}
