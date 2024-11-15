namespace MauiApp1.ViewModels;
using System.ComponentModel;
using MauiApp1.Models;

public class AddSectionsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private List<Section> sections;
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Section> Sections{
        get => sections;
        set{
            sections = value;
            OnPropertyChanged(nameof(Sections));
        }
    }

    public AddSectionsViewModel(string netid)
    {
        databaseService = new DatabaseService();
        GetSectionsAsync(netid);
    }

    public async void GetSectionsAsync(string netid)
    {
        Sections = await databaseService.GetSections(netid);
    }

    public async Task<string> AddSectionAsync(string netid, List<string> sectionInfo, List<DateTime> dates)
    {
        string sectionResultMessage = await databaseService.AddSection(netid, sectionInfo, dates);
        Console.WriteLine(sectionResultMessage);
        return sectionResultMessage;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}