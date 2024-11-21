/*
    PRSetUpViewModel:
        Handles creating the peer reviews and the criteria to then be populated in the database
        based on section given
    
    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID: sny200000

*/

namespace MauiApp1.ViewModels;
using System.ComponentModel;
using MauiApp1.Models;
public class PRSetUpViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;

    private List<Criteria> criterias;
    public List<PeerReview> peerReviews;
    public event PropertyChangedEventHandler PropertyChanged;

    public List<Criteria> Criterias{
        get => criterias;
        set{
            criterias = value;
            OnPropertyChanged(nameof(Criterias));
        }
    }

     public List<PeerReview> PeerReviews{
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
    public async void GetPRAsync(string code){
        PeerReviews = await databaseService.GetPeerReviews(code);
    }

    //Retrieves professors current criteria for a section
    public async void GetCriteriaAsync(string section){
        Criterias = await databaseService.GetSectionsCriteria(section);
        
    }

    // Calls the method in the Database.cs model to create a new criteria for a peer review
    public async Task<string> CreateCriteriaAsync(string netid, List<string> setupInfo)
    {
        // Get the message from the stored procedure (success or error message)
        string criteriaResultMessage = await databaseService.CreateCriteria(netid, setupInfo);

        // Log the result (or use it in the UI)
        Console.WriteLine(criteriaResultMessage);

        return criteriaResultMessage;
    }
    // Calls the method in the Database.cs model to create a new peer review for a section
    public async Task<string> PRAsync(string netid, List<string> PRDetails, List<DateOnly> dates)
    {
        // Get the message from the stored procedure (success or error message)
        string prResultMessage = await databaseService.CreatePeerReview(netid, PRDetails, dates);

        // Log the result (or use it in the UI)
        Console.WriteLine(prResultMessage);

        return prResultMessage;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}
