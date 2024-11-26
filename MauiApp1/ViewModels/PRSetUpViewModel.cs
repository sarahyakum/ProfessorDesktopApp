/*
    PRSetUpViewModel:
        Handles creating the peer reviews and the criteria to then be populated in the database
        based on section given
    
    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID: sny200000

    Worked on by Emma Hockett for CS 4485.0W1, Senior Design Project, Started on November 20, 2024
        NETID: ech210001

*/

namespace MauiApp1.ViewModels;
using System.ComponentModel;
using MauiApp1.Models;
public class PRSetUpViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService databaseService;

    private List<Criteria>? criterias;
    public List<PeerReview>? peerReviews;
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Criteria>? Criterias{
        get => criterias;
        set{
            criterias = value;
            OnPropertyChanged(nameof(Criterias));
        }
    }

     public List<PeerReview>? PeerReviews{
        get => peerReviews;
        set{
            peerReviews = value;
            OnPropertyChanged(nameof(PeerReviews));
        }
    }

    public PRSetUpViewModel(string netid, string code)
    {
        databaseService = new DatabaseService();
        GetCriteriaAsync(code);
        GetPRAsync(code);

    }


    //Get current sections peer reviews
    // Written by Sarah Yakum (sny200000)
    public async void GetPRAsync(string code){
        PeerReviews = await databaseService.GetPeerReviews(code);
    }


    //Retrieves professors current criteria for a section
    // Written by Sarah Yakum (sny200000)
    public async void GetCriteriaAsync(string code){
        Criterias = await databaseService.GetSectionsCriteria(code);
    }


    // Calls the method in the Database.cs model to create a new criteria for a peer review
    // Written by Sarah Yakum(sny200000)
    public async Task<string> CreateCriteriaAsync(string netid, List<string> setupInfo, string code)
    {
        // Get the message from the stored procedure (success or error message)
        string criteriaResultMessage = await databaseService.CreateCriteria(netid, setupInfo);
        GetCriteriaAsync(code);

        return criteriaResultMessage;
    }


    // Calls the method in the Database.cs model to create a new peer review for a section
    // Written by Sarah Yakum (sny200000)
    public async Task<string> PRAsync(string netid, List<string> PRDetails, List<DateTime> dates, string code)
    {
        // Get the message from the stored procedure (success or error message)
        string prResultMessage = await databaseService.CreatePeerReview(netid, PRDetails, dates);
        GetPRAsync(code);
        return prResultMessage;
    }


    // Calls the method in the Database.cs model to edit a criteria
    // Written by Emma Hockett (ech210001)
    public async Task<string>EditCriteriaAsync(string section, Criteria oldCriteria, Criteria newCriteria)
    {
        int oldCriteriaID = await databaseService.GetCriteriaID(section, oldCriteria.name, oldCriteria.description ?? string.Empty, oldCriteria.reviewType);
        if(oldCriteriaID != -1)
        {
            string editResultMessage = await databaseService.EditCriteria(section, oldCriteriaID, newCriteria.name, newCriteria.description ?? string.Empty, newCriteria.reviewType);
            GetCriteriaAsync(section);
            return editResultMessage;
        }
        else{
            return "Could not find criteria";
        }
    }


    // Calls the method in the Database.cs to check whether a critera is being used in a Peer Review already 
    // Wrutten by Emma Hockett (ech210001)
    public async Task<string>CheckCriteriaInPRAsync(string section, string reviewType)
    {
        string  statusMessage = await databaseService.CheckCriteriaInPR(section, reviewType);
        GetCriteriaAsync(section);
        return statusMessage;
    }


    // Calls the method in the Database.cs model to delete a criterion
    // Written by Emma Hockett (ech210001)
    public async Task<string>DeleteCriteriaAsync(string section, string name, string reviewType)
    {
        string  statusMessage = await databaseService.DeleteCriteria(section, name, reviewType);
        GetCriteriaAsync(section);
        return statusMessage;
    }


    // Calls the method in the Database.cs model to edit a peer reviews dates 
    // Written by Emma Hockett (ech210001)
    public async Task<string>EditPRDatesAsync(string section, string type, DateOnly startDate, DateOnly endDate)
    {
        string  statusMessage = await databaseService.EditPRDates(section, type, startDate, endDate);
        GetPRAsync(section);
        return statusMessage;
    }


    // Calls the method in the Database.cs model to delete a peer review
    // Written by Emma Hockett (ech210001)
    public async Task<string>DeletePRAsync(string section, string type)
    {
        string  statusMessage = await databaseService.DeletePR(section, type);
        GetPRAsync(section);
        return statusMessage;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}
