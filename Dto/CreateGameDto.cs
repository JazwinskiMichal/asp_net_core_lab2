using System.ComponentModel.DataAnnotations;

namespace GameStoreMono.BlazorServer.Dto;

/// <summary>
/// Data transfer object for creating a new game
/// </summary>
public class CreateGameDto
{
    /// <summary>
    /// The name of the game
    /// </summary>
    [Required(ErrorMessage = "Game name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Game name must be between 1 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// The unique identifier of the game's genre
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Please select a genre")]
    public int GenreId { get; set; }
    
    /// <summary>
    /// The price of the game in USD
    /// </summary>
    [Range(0.01, 999.99, ErrorMessage = "Price must be between $0.01 and $999.99")]
    public decimal Price { get; set; }
    
    /// <summary>
    /// The release date of the game
    /// </summary>
    [Required(ErrorMessage = "Release date is required")]
    public DateOnly ReleaseDate { get; set; }

    // Parameterless constructor for form binding
    public CreateGameDto() { }

    // Constructor for convenience
    public CreateGameDto(string name, int genreId, decimal price, DateOnly releaseDate)
    {
        Name = name;
        GenreId = genreId;
        Price = price;
        ReleaseDate = releaseDate;
    }
}