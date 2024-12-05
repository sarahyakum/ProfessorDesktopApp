/*
    Email Students View Model:
        Relationship between the email students page and the database
        Pulls the students who have either:
            - Not entered any timeslots for the week
            - Not completed the current peer review 

    Written entirely by Emma Hockett for CS 4485.0W1, Started on November 21, 2024
        NetID" ech21001
*/

using System.ComponentModel;
using System.Collections.ObjectModel;
using CS4485_Team75.Models;
namespace CS4485_Team75.ViewModels;

public class EmailStudentsViewModel : INotifyPropertyChanged
{
    public readonly string professorID;
    private readonly  DatabaseService databaseService;
    private List<Section>? sections;
    private ObservableCollection<SectionWithEmails>? _TTsections;
    private ObservableCollection<SectionWithEmails>? _PRsections;
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Section>? Sections{
        get => sections;
        set{
            sections=value;
            OnPropertyChanged(nameof(Sections));
        }
    }

    public ObservableCollection<SectionWithEmails>? TTsections
    {
        get => _TTsections;
        set{
            _TTsections=value;
            OnPropertyChanged(nameof(TTsections));
        }
    }

    public ObservableCollection<SectionWithEmails>? PRsections
    {
        get => _PRsections;
        set{
            _PRsections=value;
            OnPropertyChanged(nameof(PRsections));
        }
    }

    public EmailStudentsViewModel(string netid)
    {
        databaseService = new DatabaseService();
        professorID = netid;
        TTsections = new ObservableCollection<SectionWithEmails>();
        PRsections = new ObservableCollection<SectionWithEmails>();
    }

    public async Task InitializeAsync()
    {
        await LoadSectionsAsync();
    }

    // Loads the sections and calls the Method to load the emails into the section 
    private async Task LoadSectionsAsync()
    {
        var timeslotSections = await LoadSectionsWithEmailsAsync(isTimeslot: true);
        if(TTsections != null)
        {
            TTsections.Clear();
        }
        else{
            TTsections = new ObservableCollection<SectionWithEmails>();
        }
        
        foreach (var section in timeslotSections)
        {
            TTsections.Add(section);
        }


        var peerReviewSections = await LoadSectionsWithEmailsAsync(isTimeslot: false);
        
        if(PRsections != null)
        {
            PRsections.Clear();
        }
        else{
            PRsections = new ObservableCollection<SectionWithEmails>();
        }

        foreach( var section in peerReviewSections)
        {
            PRsections.Add(section);
        }

        OnPropertyChanged(nameof(TTsections));
        OnPropertyChanged(nameof(PRsections));
    }

    // Retrieves the emails of the students for the Timetracking and Peer Review
    private async Task<List<SectionWithEmails>> LoadSectionsWithEmailsAsync(bool isTimeslot)
    {
        Sections = await databaseService.GetSections(professorID);
        var sectionWithEmails = new List<SectionWithEmails>();

        DateTime today = DateTime.Now;
        int daysToSubtract = (int)today.DayOfWeek;

        DateTime firstDayOfWeek = today.AddDays(-daysToSubtract);
        DateOnly firstDay = DateOnly.FromDateTime(firstDayOfWeek);

        List<string> retrievedEmails = new List<string>();

        foreach(var section in Sections)
        {
            if(isTimeslot)
            {
                retrievedEmails = await databaseService.GetTTEmails(section.code, firstDay);
            }
            else{
                retrievedEmails = await databaseService.GetPREmails(section.code);
            }

            sectionWithEmails.Add(new SectionWithEmails
            {
                name = section.name, 
                emails = string.Join("\n", retrievedEmails)
            });
        }

        return sectionWithEmails;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}



// Wrapper class of the section with the name of the section and the student emails 
public class SectionWithEmails
{
    public required string name { get; set;}
    public string? emails { get; set;}
}