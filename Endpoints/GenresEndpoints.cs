using GameStoreMono.BlazorServer.Contracts;
using GameStoreMono.BlazorServer.Data;
using GameStoreMono.BlazorServer.Entities;
using GameStoreMono.BlazorServer.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMono.BlazorServer.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres")
            .WithTags("Genres")
            .WithOpenApi();

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                            .Select(genre => genre.ToDto())
                            .AsNoTracking()
                            .ToListAsync())
        .WithName("GetAllGenres")
        .WithSummary("Get all genres")
        .WithDescription("Retrieves a list of all available game genres")
        .Produces<List<GenreDto>>(StatusCodes.Status200OK)
        .WithOpenApi();

        group.MapPost("/", async (GenreDto genreDto, GameStoreContext dbContext) =>
        {
            var genre = new GenreEntity
            {
                Name = genreDto.Name
            };

            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                "GetGenreById",
                new { id = genre.Id },
                genre.ToDto()
            );
        })
        .WithName("CreateGenre")
        .WithSummary("Create a new genre")
        .WithDescription("Creates a new genre")
        .Produces<GenreDto>(StatusCodes.Status201Created)
        .WithOpenApi();

        return group;
    }
}