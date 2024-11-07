/*
    LoginViewModel Class
        Handles the authentication between the user interface and the database for the professor login

        NOT SURE WHAT TO ADD HERE 
    
    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID:

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

    // Calls the method in the Database.cs model to check whether the given information is a correct login
    public async Task<string> CriteriaAsync(string netid, List<string> setupInfo)
    {
        // Get the message from the stored procedure (success or error message)
        string criteriaResultMessage = await databaseService.CreateCriteria(netid, setupInfo);

        // Log the result (or use it in the UI)
        Console.WriteLine(criteriaResultMessage);

        return criteriaResultMessage;
    }

    
}
