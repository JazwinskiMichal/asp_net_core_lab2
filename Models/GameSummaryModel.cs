using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameStoreMono.BlazorServer.Dto;

namespace GameStoreMono.BlazorServer.Models;

public class GameSummaryModel : INotifyPropertyChanged
{
    private Guid _id;
    private string _name = string.Empty;
    private string _genre = string.Empty;
    private decimal _price;
    private DateOnly _releaseDate;

    public Guid Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }
    }

    public string Genre
    {
        get => _genre;
        set
        {
            if (_genre != value)
            {
                _genre = value;
                NotifyPropertyChanged(nameof(Genre));
            }
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                NotifyPropertyChanged(nameof(Price));
            }
        }
    }

    public DateOnly ReleaseDate
    {
        get => _releaseDate;
        set
        {
            if (_releaseDate != value)
            {
                _releaseDate = value;
                NotifyPropertyChanged(nameof(ReleaseDate));
            }
        }
    }

    public void UpdateModel(List<GameSummaryDto> gameSummaryDtos)
    {
        foreach (var dto in gameSummaryDtos)
        {
            Id = dto.Id;
            Name = dto.Name;
            Genre = dto.Genre;
            Price = dto.Price;
            ReleaseDate = dto.ReleaseDate;
        }

        NotifyPropertyChanged(nameof(Id));
        NotifyPropertyChanged(nameof(Name));
        NotifyPropertyChanged(nameof(Genre));
        NotifyPropertyChanged(nameof(Price));
        NotifyPropertyChanged(nameof(ReleaseDate));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
