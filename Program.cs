
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


app.MapGet("/games", () => games);

app.Run();
