using GameStore.Dtos;

namespace GameStore.Entities;

public static class EntityExtensions
{
    public static GameDTO AsDto(this Game game)
    {
        return new GameDTO(
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.ReleaseDate,
            game.ImageUri
        );
    }
}