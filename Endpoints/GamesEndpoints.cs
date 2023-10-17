using GameStore.Entities;
using GameStore.Repositories;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var gamesGroup = routes.MapGroup("/games").WithParameterValidation();


        gamesGroup.MapGet("/", (IGamesRepository gameRepo) => gameRepo.GetAll());
        gamesGroup.MapGet("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? game = gameRepo.Get(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game);
        }).WithName("GetGame");

        gamesGroup.MapPost("/", (IGamesRepository gameRepo, Game game) =>
        {
            gameRepo.Create(game);

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
        });

        gamesGroup.MapPut("/{id}", (IGamesRepository gameRepo, int id, Game updatedGame) =>
        {
            Game? existingGame = gameRepo.Get(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            gameRepo.Update(id, updatedGame);

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