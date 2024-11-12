/*
   SectionsViewModel Class 
        Getting the data of each section for the professor

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID:
*/
namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;



public class SectionsViewModel : INotifyPropertyChanged
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



    public SectionsViewModel(string netid)
    {
    
        databaseService = new DatabaseService();
        LoadSectionsAsync(netid);
        

    }

    private async Task LoadSectionsAsync(string netid)
    {
        Sections = await databaseService.GetSections(netid);
        
    }


    
    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
