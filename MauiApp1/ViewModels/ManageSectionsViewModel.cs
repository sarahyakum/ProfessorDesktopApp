/*
    Add Sections View Model 
        The relationship between the page and the database for the add sections

    Written etirely by Emma Hockett for CS 4485.0W1, Started on November 15, 2024
        NetID" ech21001
*/

namespace MauiApp1.ViewModels;
using System.ComponentModel;
using MauiApp1.Models;

public class ManageSectionsViewModel : INotifyPropertyChanged
{
    public string professorid;
    private readonly DatabaseService databaseService;
    private List<Section>? sections;
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Section>? Sections{
        get => sections;
        set{
            sections = value;
            OnPropertyChanged(nameof(Sections));
        }
    }

    public ManageSectionsViewModel(string netid)
    {
        databaseService = new DatabaseService();
        professorid = netid;
    }

    public async Task InitializeAsync()
    {
        await GetSectionsAsync();
    }

    // Pulls all of the section information from the database
    public async Task GetSectionsAsync()
    {
        Sections = await databaseService.GetSections(professorid);
    }


    // Takes the information for adding the section and passes it to the database 
    public async Task<string> AddSectionAsync(string netid, List<string> sectionInfo, List<DateOnly> dates)
    {
        string sectionResultMessage = await databaseService.AddSection(netid, sectionInfo, dates);
        await GetSectionsAsync();
        return sectionResultMessage;
    }


    // Takes the information from the edited section and passes it to the database 
    public async Task<string> EditSectionAsync(string section, List<string> sectionInfo, List<DateOnly> dates)
    {
        string sectionResultMessage = await databaseService.EditSection(section, sectionInfo, dates);
        await GetSectionsAsync();
        return sectionResultMessage;
    }


    // Takes the information about the section being deleted and passes it to the database 
    public async Task<string> DeleteSectionAsync(string section)
    {
        string sectionResultMessage = await databaseService.DeleteSection(section);
        await GetSectionsAsync();
        return sectionResultMessage;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}