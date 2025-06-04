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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartMonitoring();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000, stoppingToken); // Check every 5 seconds
            await CheckForPlcDataUpdates();
        }
    }

    public Task StartMonitoring()
    {
        _logger.LogInformation("PLC Data monitoring started");
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

            // Use your existing GameService to get games
            var updatedGames = await gameService.GetGamesAsync();

            if (updatedGames.Count == 0)
            {
                _logger.LogInformation("No games found in database");
                return;
            }

            // Update the game collection model - this will trigger UI updates
            _gameCollectionModel.UpdateGames(updatedGames);
            _logger.LogInformation("PLC data updated with {Count} games", updatedGames.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking PLC data");
        }
    }
}