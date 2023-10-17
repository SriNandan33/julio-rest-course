using GameStore.Entities;

namespace GameStore.Repositories;

public class InMemGamesRepository : IGamesRepository
{
    private List<Game> games = new()
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

    public IEnumerable<Game> GetAll()
    {
        return games;
    }

    public Game? Get(int id)
    {
        return games.Find(game => game.Id == id);
    }

    public void Create(Game game)
    {
        game.Id = games.Count() + 1;
        games.Add(game);
    }

    public void Update(int id, Game updatedGame)
    {
        int index = games.FindIndex(game => game.Id == id);
        updatedGame.Id = id;
        games[index] = updatedGame;
    }

    public void Delete(int id)
    {
        int index = games.FindIndex(game => game.Id == id);
        games.RemoveAt(index);
    }
}