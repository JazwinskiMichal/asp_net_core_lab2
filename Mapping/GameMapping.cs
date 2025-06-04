
using GameStoreMono.BlazorServer.Dto;
using GameStoreMono.BlazorServer.Entities;
using GameStoreMono.BlazorServer.Models;

namespace GameStoreMono.BlazorServer.Mapping;

public static class GameMapping
{
    public static GameSummaryDto ToGameSummaryDto(this GameEntity game)
    {
        ArgumentNullException.ThrowIfNull(game);

        return new GameSummaryDto(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsDto ToGameDetailsDto(this GameEntity game)
    {
        ArgumentNullException.ThrowIfNull(game);

        return new GameDetailsDto(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameEntity ToEntity(this CreateGameDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new GameEntity
        {
            Name = dto.Name,
            GenreId = dto.GenreId,
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate
        };
    }

    public static GameEntity ToEntity(this UpdateGameDto dto, Guid id)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new GameEntity
        {
            Id = id,
            Name = dto.Name,
            GenreId = dto.GenreId,
            Price = dto.Price ?? 0, // Assuming Price is required in UpdateGameDto
            ReleaseDate = dto.ReleaseDate ?? DateOnly.MinValue // Assuming ReleaseDate is optional
        };
    }

    public static GameSummaryModel ToGameSummaryModel(this GameEntity game)
    {
        ArgumentNullException.ThrowIfNull(game);

        return new GameSummaryModel
        {
            Id = game.Id,
            Name = game.Name,
            Genre = game.Genre!.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }
}
