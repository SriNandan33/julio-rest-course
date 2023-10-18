using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record GameDTO(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string ImageUri
);

public record CreateGameDTO(
    [Required][StringLength(100)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(1, 100)] decimal Price,
    DateTime ReleaseDate,
    [Url][StringLength(100)] string ImageUri
);

public record UpdateGameDTO(
    [Required][StringLength(100)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(1, 100)] decimal Price,
    DateTime ReleaseDate,
    [Url][StringLength(100)] string ImageUri
);