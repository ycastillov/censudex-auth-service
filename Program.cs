using censudex_auth_service.Src.Helpers;
using censudex_auth_service.Src.Interfaces;
using censudex_auth_service.Src.Services;
using ClientsService.Grpc;
using Grpc.Net.Client;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var redis = ConnectionMultiplexer.Connect(
    builder.Configuration.GetConnectionString("Redis:Connection")!
);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<ILogoutService, LogoutService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ClientsGrpc.ClientsGrpcClient(
        GrpcChannel.ForAddress(config["Grpc:ClientsService"]!)
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
