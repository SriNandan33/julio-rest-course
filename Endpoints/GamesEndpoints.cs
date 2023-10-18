using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Repositories;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var gamesGroup = routes.MapGroup("/games").WithParameterValidation();


        gamesGroup.MapGet("/", (IGamesRepository gameRepo) => gameRepo.GetAll().Select(game => game.AsDto()));
        gamesGroup.MapGet("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? game = gameRepo.Get(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game.AsDto());
        }).WithName("GetGame");

        gamesGroup.MapPost("/", (IGamesRepository gameRepo, CreateGameDTO createGameDTO) =>
        {
            Game game = new()
            {
                Name = createGameDTO.Name,
                Genre = createGameDTO.Genre,
                Price = createGameDTO.Price,
                ReleaseDate = createGameDTO.ReleaseDate,
                ImageUri = createGameDTO.ImageUri
            };
            gameRepo.Create(game);

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.AsDto());
        });

        gamesGroup.MapPut("/{id}", (IGamesRepository gameRepo, int id, UpdateGameDTO updatedGameDto) =>
        {
            Game? existingGame = gameRepo.Get(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGameDto.Name;
            existingGame.Price = updatedGameDto.Price;
            existingGame.Genre = updatedGameDto.Genre;
            existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
            existingGame.ImageUri = updatedGameDto.ImageUri;

            gameRepo.Update(id, existingGame);

            return Results.NoContent();
        });

        gamesGroup.MapDelete("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? existingGame = gameRepo.Get(id);

            if (existingGame is not null)
            {
                gameRepo.Delete(id);
            }
            return Results.NoContent();
        });

        return gamesGroup;
    }
}