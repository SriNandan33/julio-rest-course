
using GameStore.Data;
using GameStore.Endpoints;
using GameStore.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IGamesRepository, GamesRepository>();

var connString = builder.Configuration.GetConnectionString("GameStoreDatabase");
builder.Services.AddSqlServer<GameStoreContext>(connString);

var app = builder.Build();

app.Services.InitializeDb();

app.MapGamesEndpoints();

app.Run();
