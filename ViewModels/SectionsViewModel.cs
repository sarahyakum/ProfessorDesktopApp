/*
   SectionsViewModel Class 
        Getting the data of each section for the professor

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on November 7, 2024
        NETID:sny200000
*/
namespace CS4485_Team75.ViewModels;

using System.ComponentModel;
using CS4485_Team75.Models;



public class SectionsViewModel : INotifyPropertyChanged
{

    private DatabaseService databaseService;
    private List<Section>? sections;
    public event PropertyChangedEventHandler? PropertyChanged;


    public List<Section> Sections{
        get => sections ?? new List<Section>();
        set{
            sections=value;
            OnPropertyChanged(nameof(Sections));
        }
        
    }

    public SectionsViewModel(string netid)
    {
        databaseService = new DatabaseService();
    }

    public async Task InitializeASync(string netid)
    {
        await LoadSectionsAsync(netid);
    }

    //Uses database service to get all the sections a professor teaches
    private async Task LoadSectionsAsync(string netid)
    {
        Sections = await databaseService.GetSections(netid);
        
    }
    
    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
