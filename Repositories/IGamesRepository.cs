using GameStore.Entities;

namespace GameStore.Repositories;

public interface IGamesRepository
{
    IEnumerable<Game> GetAll();
    Game? Get(int id);
    void Create(Game game);
    void Update(int id, Game updatedGame);
    void Delete(int id);
}