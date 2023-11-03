using System.Diagnostics;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var gamesGroup = routes.MapGroup("/games").WithParameterValidation();


        gamesGroup.MapGet("/", (IGamesRepository gameRepo, ILoggerFactory loggerFactory) =>
        {
            try
            {
                return Results.Ok(gameRepo.GetAll().Select(game => game.AsDto()));
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("GamesEndpoint");
                var traceId = Activity.Current?.TraceId;
                logger.LogError(ex, "Cloud not process a request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, traceId);

                return Results.Problem(title: "We made a mistake but we are working on it!", statusCode: StatusCodes.Status500InternalServerError, extensions: new Dictionary<string, object?>{
                    {"TraceID", traceId.ToString()}
                });
            }
        });
        gamesGroup.MapGet("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? game = gameRepo.Get(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game.AsDto());
        }).WithName("GetGame")
        .RequireAuthorization("GamesRead");

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
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"));

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
        })
        .RequireAuthorization("GamesWrite");

        gamesGroup.MapDelete("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? existingGame = gameRepo.Get(id);

            if (existingGame is not null)
            {
                gameRepo.Delete(id);
            }
            return Results.NoContent();
        })
        .RequireAuthorization("GamesWrite");

        return gamesGroup;
    }
}