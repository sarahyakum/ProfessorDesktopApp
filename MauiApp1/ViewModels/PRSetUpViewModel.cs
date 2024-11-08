/*
    PRSetUpViewModel:
        Handles creating the peer reviews and the criteria to then be populated in the database
        based on section given
    
    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID: sny200000

*/

namespace MauiApp1.ViewModels;
using MauiApp1.Models;
public class PRSetUpViewModel
{
    private DatabaseService databaseService;

    public PRSetUpViewModel()
    {
        databaseService = new DatabaseService();
    }

    // Calls the method in the Database.cs model to create a new criteria for a peer review
    public async Task<string> CriteriaAsync(string netid, List<string> setupInfo)
    {
        // Get the message from the stored procedure (success or error message)
        string criteriaResultMessage = await databaseService.CreateCriteria(netid, setupInfo);

        // Log the result (or use it in the UI)
        Console.WriteLine(criteriaResultMessage);

        return criteriaResultMessage;
    }
    // Calls the method in the Database.cs model to create a new peer review for a section
    public async Task<string> PRAsync(string netid, List<string> PRDetails, List<DateTime> dates)
    {
        // Get the message from the stored procedure (success or error message)
        string prResultMessage = await databaseService.CreatePeerReview(netid, PRDetails, dates);

        // Log the result (or use it in the UI)
        Console.WriteLine(prResultMessage);

        return prResultMessage;
    }

    
}
