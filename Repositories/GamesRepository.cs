using GameStore.Data;
using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories;

public class GamesRepository : IGamesRepository
{
    private readonly GameStoreContext dbContext;
    private readonly ILogger<GamesRepository> logger;
    public GamesRepository(GameStoreContext dbContext, ILogger<GamesRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public IEnumerable<Game> GetAll()
    {
        return dbContext.Games.AsNoTracking().ToList();
    }

    public Game? Get(int id)
    {
        return dbContext.Games.Find(id);
    }

    public void Create(Game game)
    {
        dbContext.Games.Add(game);
        dbContext.SaveChanges();

        logger.LogInformation("Created game {Name} with price {Price}", game.Name, game.Price);
    }

    public void Update(int id, Game updatedGame)
    {
        dbContext.Games.Update(updatedGame);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        dbContext.Games.Where(game => game.Id == id)
            .ExecuteDelete();
    }
}