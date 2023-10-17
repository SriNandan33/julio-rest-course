
using GameStore.Entities;

List<Game> games = new()
{
    new Game(){
        Id = 1,
        Name = "PubG",
        Price = 20.5M,
        Genre = "Shooting",
        ImageUri = "https://placehold.co/100",
        ReleaseDate = new DateTime(2018, 1, 1)
    },
    new Game(){
        Id = 2,
        Name = "FreeFire",
        Price = 20.5M,
        Genre = "Shooting",
        ImageUri = "https://placehold.co/100",
        ReleaseDate = new DateTime(2019, 1, 1)
    },
    new Game(){
        Id = 3,
        Name = "NFS - Need for Speed",
        Price = 50.5M,
        Genre = "Racing",
        ImageUri = "https://placehold.co/100",
        ReleaseDate = new DateTime(2015, 1, 1)
    },
    new Game(){
        Id = 4,
        Name = "Chess 2.0",
        Price = 5.5M,
        Genre = "Board",
        ImageUri = "https://placehold.co/100",
        ReleaseDate = new DateTime(2018, 1, 1)
    },
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var gamesGroup = app.MapGroup("/games").WithParameterValidation();


gamesGroup.MapGet("/", () => games);
gamesGroup.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
}).WithName("GetGame");

gamesGroup.MapPost("/", (Game game) =>
{
    game.Id = games.Count() + 1;
    games.Add(game);

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

gamesGroup.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? existingGame = games.Find(game => game.Id == id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Price = updatedGame.Price;
    existingGame.ImageUri = updatedGame.ImageUri;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
});

gamesGroup.MapDelete("/{id}", (int id) =>
{
    Game? existingGame = games.Find(game => game.Id == id);

    if (existingGame is not null)
    {
        games.Remove(existingGame);
    }
    return Results.NoContent();
});

app.Run();
