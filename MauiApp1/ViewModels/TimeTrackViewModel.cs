namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Pages;

public class TimeTrackViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private List<Section> sections;
    public event PropertyChangedEventHandler PropertyChanged;

    public List<Section> Sections{
        get => sections;
        set{
            sections=value;
            OnPropertyChanged(nameof(Sections));
        }
        
        }

    public TimeTrackViewModel(string netid)
    {
        databaseService = new DatabaseService();
        LoadSectionsAsync(netid);

    }

    private async void LoadSectionsAsync(string netid)
    {
        Sections = await databaseService.getSections(netid);
        
    }
    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
