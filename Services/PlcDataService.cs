using System.Collections.ObjectModel;
using GameStoreMono.BlazorServer.Interfaces;
using GameStoreMono.BlazorServer.Models;

namespace GameStoreMono.BlazorServer.Services;

public class PlcDataService(GameCollectionModel gameCollectionModel,
                            ILogger<PlcDataService> logger,
                            IServiceScopeFactory scopeFactory) : BackgroundService, IPlcDataService
{
    private readonly GameCollectionModel _gameCollectionModel = gameCollectionModel;
    private readonly ILogger<PlcDataService> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartMonitoring();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_pollingInterval, stoppingToken);
                await CheckForPlcDataUpdates();
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in PLC monitoring loop");
                // Continue monitoring even if there's an error
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        
        await StopMonitoring();
    }

    public Task StartMonitoring()
    {
        _logger.LogInformation("PLC Data monitoring started (polling every {Interval})", _pollingInterval);
        return Task.CompletedTask;
    }

    public Task StopMonitoring()
    {
        _logger.LogInformation("PLC Data monitoring stopped");
        return Task.CompletedTask;
    }

    private async Task CheckForPlcDataUpdates()
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var gameService = scope.ServiceProvider.GetRequiredService<GameService>();

            var updatedGames = await gameService.GetGamesAsync();

            if (updatedGames.Count == 0)
            {
                _logger.LogDebug("No games found in database");
                return;
            }

            // Only update if the data has actually changed
            if (!HasDataChanged(updatedGames))
            {
                _logger.LogDebug("No changes detected in game data");
                return;
            }

            _gameCollectionModel.UpdateGames(updatedGames);
            _logger.LogInformation("PLC data updated with {Count} games", updatedGames.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking PLC data updates");
        }
    }

    private bool HasDataChanged(ObservableCollection<GameSummaryModel> newGames)
    {
        var currentGames = _gameCollectionModel.Games;
        
        // Simple comparison - in production you might want more sophisticated change detection
        if (currentGames.Count != newGames.Count)
            return true;

        return newGames.Any(newGame => 
            !currentGames.Any(existing => 
                existing.Id == newGame.Id && 
                existing.Name == newGame.Name && 
                existing.Price == newGame.Price));
    }
}