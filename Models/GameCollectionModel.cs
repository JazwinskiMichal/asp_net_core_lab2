using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameStoreMono.BlazorServer.Dto;

namespace GameStoreMono.BlazorServer.Models;

public class GameCollectionModel : INotifyPropertyChanged
{
    private ObservableCollection<GameSummaryModel> _games = [];
    private DateTime? _lastUpdateTime;

    public ObservableCollection<GameSummaryModel> Games
    {
        get => _games;
        private set
        {
            if (_games != value)
            {
                _games = value;
                NotifyPropertyChanged();
            }
        }
    }

    public DateTime? LastUpdateTime
    {
        get => _lastUpdateTime;
        private set
        {
            if (_lastUpdateTime != value)
            {
                _lastUpdateTime = value;
                NotifyPropertyChanged();
            }
        }
    }

    public void UpdateGames(ObservableCollection<GameSummaryModel> newGames)
    {
        // Clear existing games
        Games.Clear();
        
        // Add new games
        foreach (var game in newGames)
        {
            Games.Add(game);
        }
        
        LastUpdateTime = DateTime.Now;
        NotifyPropertyChanged(nameof(Games));
        NotifyPropertyChanged(nameof(LastUpdateTime));
    }

    public void UpdateGames(List<GameSummaryDto> gameDtos)
    {
        // Clear existing games
        Games.Clear();
        
        // Convert DTOs to Models and add them
        foreach (var dto in gameDtos)
        {
            var model = new GameSummaryModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Genre = dto.Genre,
                Price = dto.Price,
                ReleaseDate = dto.ReleaseDate
            };
            Games.Add(model);
        }
        
        LastUpdateTime = DateTime.Now;
        NotifyPropertyChanged(nameof(Games));
        NotifyPropertyChanged(nameof(LastUpdateTime));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}