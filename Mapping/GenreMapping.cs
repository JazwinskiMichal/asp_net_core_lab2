using GameStoreMono.BlazorServer.Dto;
using GameStoreMono.BlazorServer.Entities;

namespace GameStoreMono.BlazorServer.Mapping;

public static class GenreMapping
{
    public static GenreDto ToDto (this GenreEntity genre)
    {
        ArgumentNullException.ThrowIfNull(genre);

        return new GenreDto(genre.Id, genre.Name);
    }
}
