using System.Text;
using censudex_auth_service.Src.Helpers;
using censudex_auth_service.Src.Interfaces;
using censudex_auth_service.Src.Services;
using ClientsService.Grpc;
using DotNetEnv;
using Grpc.Net.Client;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Load .env
// -----------------------------
Env.Load();

// -----------------------------
// OpenAPI
// -----------------------------
builder.Services.AddOpenApi();

// -----------------------------
// JWT Settings from ENV
// -----------------------------
builder.Services.Configure<JwtSettings>(options =>
{
    options.Key = Environment.GetEnvironmentVariable("JWT_KEY")!;
    options.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
    options.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
    options.ExpiresInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES")!);
});

// Register TokenValidationParameters
builder.Services.AddSingleton<TokenValidationParameters>(sp =>
{
    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)
    );

    return new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        ClockSkew = TimeSpan.Zero,
    };
});

// -----------------------------
// Redis
// -----------------------------
var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost:6379";
var options = ConfigurationOptions.Parse(redisConnection);
options.AbortOnConnectFail = false; // Reintentos automáticos

IConnectionMultiplexer? redis = null;
try
{
    redis = ConnectionMultiplexer.Connect(options);
    Console.WriteLine("Redis conectado exitosamente!");
}
catch (Exception ex)
{
    Console.WriteLine($"No se pudo conectar a Redis: {ex.Message}");
    Console.WriteLine("Asegúrate de ejecutar: docker-compose up -d");
    throw;
}

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// -----------------------------
// Auth Services
// -----------------------------
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ILogoutService, LogoutService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// -----------------------------
// gRPC Client for Clients Service
// -----------------------------
builder.Services.AddSingleton(sp =>
{
    var grpcUrl = Environment.GetEnvironmentVariable("GRPC_CLIENTS_URL")!;
    return new ClientsGrpc.ClientsGrpcClient(GrpcChannel.ForAddress(grpcUrl));
});

var app = builder.Build();

// -----------------------------
// Pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
