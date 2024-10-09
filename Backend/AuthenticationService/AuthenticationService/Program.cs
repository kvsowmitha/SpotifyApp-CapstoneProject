using System.Text;
using AuthenticationService;
using AuthenticationService.Middleware;
using AuthenticationService.Model;
using AuthenticationService.Repository;
using AuthenticationService.Services;
using AuthenticationService.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
           });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<AuthView>();
builder.Services.AddScoped<AuthRepo>();
builder.Services.AddConsul(builder.Configuration);
builder.Services.AddCors(options => options.AddPolicy("AngularClient", policy =>
{
    policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<ConsumerService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
//builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddDbContext<AuthDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();
app.UseCors("AngularClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Register Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.UseConsul(app.Configuration);
app.Run();
