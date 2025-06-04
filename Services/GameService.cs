using GameStoreMono.BlazorServer.Contracts;
using GameStoreMono.BlazorServer.Data;
using GameStoreMono.BlazorServer.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMono.BlazorServer.Services;

public class GameService(GameStoreContext context, ILogger<GameService> logger)
{
    private readonly GameStoreContext _context = context;
    private readonly ILogger<GameService> _logger = logger;

    public async Task<List<GameSummaryDto>> GetGamesAsync()
    {
        try
        {
            _logger.LogInformation("Fetching games from database...");
            
            var games = await _context.Games
                .Include(g => g.Genre)
                .Select(g => g.ToGameSummaryDto())
                .AsNoTracking()
                .ToListAsync();
                
            _logger.LogInformation("Successfully fetched {Count} games", games.Count);
            return games;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching games from database");
            return new List<GameSummaryDto>();
        }
    }

    public async Task<GameDetailsDto?> GetGameAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching game {GameId} from database...", id);
            
            var game = await _context.Games.FindAsync(id);
            return game?.ToGameDetailsDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching game {GameId}", id);
            return null;
        }
    }

    public async Task<bool> DeleteGameAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting game {GameId}...", id);
            
            var rowsAffected = await _context.Games
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();
                
            var success = rowsAffected > 0;
            
            if (success)
            {
                _logger.LogInformation("Successfully deleted game {GameId}", id);
            }
            else
            {
                _logger.LogWarning("No game found with ID {GameId} to delete", id);
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting game {GameId}", id);
            return false;
        }
    }

    public async Task<GameDetailsDto?> CreateGameAsync(CreateGameDto createGameDto)
    {
        try
        {
            _logger.LogInformation("Creating new game: {GameName}", createGameDto.Name);
            
            var game = createGameDto.ToEntity();
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Successfully created game {GameId}", game.Id);
            return game.ToGameDetailsDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating game");
            return null;
        }
    }

    public async Task<GameDetailsDto?> UpdateGameAsync(Guid id, UpdateGameDto updateGameDto)
    {
        try
        {
            _logger.LogInformation("Updating game {GameId}", id);
            
            var existingGame = await _context.Games.FindAsync(id);
            if (existingGame == null)
            {
                _logger.LogWarning("Game {GameId} not found for update", id);
                return null;
            }

            var updatedGame = updateGameDto.ToEntity(id);
            _context.Entry(existingGame).CurrentValues.SetValues(updatedGame);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated game {GameId}", id);
            return updatedGame.ToGameDetailsDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating game {GameId}", id);
            return null;
        }
    }
}