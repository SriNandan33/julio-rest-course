using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Repositories;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{

    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var gamesGroup = routes
                            .NewVersionedApi()
                            .MapGroup("/games")
                            .HasApiVersion(1.0)
                            .HasApiVersion(2.0)
                            .WithParameterValidation();


        gamesGroup.MapGet("/", (IGamesRepository gameRepo, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok(gameRepo.GetAll().Select(game => game.AsDtoV1()));
        })
        .MapToApiVersion(1.0);

        gamesGroup.MapGet("/", (IGamesRepository gameRepo, ILoggerFactory loggerFactory) =>
        {
            return Results.Ok(gameRepo.GetAll().Select(game => game.AsDtoV2()));
        })
        .MapToApiVersion(2.0);

        gamesGroup.MapGet("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? game = gameRepo.Get(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game.AsDtoV1());
        })
        .WithName("GetGame")
        .RequireAuthorization("GamesRead")
        .MapToApiVersion(1.0);

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

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.AsDtoV1());
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .MapToApiVersion(1.0);

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
        .RequireAuthorization("GamesWrite")
        .MapToApiVersion(1.0);

        gamesGroup.MapDelete("/{id}", (IGamesRepository gameRepo, int id) =>
        {
            Game? existingGame = gameRepo.Get(id);

            if (existingGame is not null)
            {
                gameRepo.Delete(id);
            }
            return Results.NoContent();
        })
        .RequireAuthorization("GamesWrite")
        .IsApiVersionNeutral();

        return gamesGroup;
    }
}