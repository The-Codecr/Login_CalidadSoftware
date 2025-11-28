using LoginBackEnd.Application.Auth;
using LoginBackEnd.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// Agregar Mongo
using MongoDB.Driver;
using LoginBackEnd.Infrastructure.Users;
using LoginBackEnd.Infrastructure.Seeders;
using LoginBackEnd.Domain.Users;

var builder = WebApplication.CreateBuilder(args);

// ***********************************************************

// Registrar MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["Mongo:ConnectionString"])
);

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["Mongo:Database"]);
});

// Inyectar User Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Registra el Seeder en servicios
builder.Services.AddSingleton<MongoUserSeeder>();


// ***********************************************************


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()      
            .AllowAnyHeader()      
            .AllowAnyMethod();    
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Mongo: Ejecutar seeding
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<MongoUserSeeder>();
    await seeder.SeedAsync();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/login", async (LoginRequest req, IAuthService auth) =>
{
    var result = await auth.LoginAsync(req);

    if (!result.Success)
        return Results.BadRequest(result);

    return Results.Ok(result);
});

app.Run();

