
using GameStore.Data;
using GameStore.Endpoints;
using GameStore.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IGamesRepository, GamesRepository>();

var connString = builder.Configuration.GetConnectionString("GameStoreDatabase");
builder.Services.AddSqlServer<GameStoreContext>(connString);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GamesRead", policy => policy.RequireClaim("scope", "games:read"));
    options.AddPolicy("GamesWrite", policy => policy.RequireClaim("scope", "games:write"));
});

var app = builder.Build();

app.Services.InitializeDb();

app.MapGamesEndpoints();

app.Run();
