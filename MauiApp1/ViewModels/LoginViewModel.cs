/*
    LoginViewModel Class
        Handles the authentication between the user interface and the database for the professor login
        and changing password

        
    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID: sny200000

*/

namespace MauiApp1.ViewModels;
using MauiApp1.Models;
public class LoginViewModel
{
    private DatabaseService databaseService;

    public LoginViewModel()
    {
        databaseService = new DatabaseService();
    }

    // Calls the method in the Database.cs model to check whether the given information is a correct login
    public async Task<string> LoginAsync(string username, string password)
    {
        // Get the message from the stored procedure (success or error message)
        string loginResultMessage = await databaseService.Login(username, password);

        // Log the result (or use it in the UI)
        Console.WriteLine(loginResultMessage);

        return loginResultMessage;
    }

    // Calls the method to change the password for the professor given they need to
    public async Task<string> ChangePasswordAsync(string netid, string oldPassword, string newPassword){
        string changeResultMessage = await databaseService.ChangePassword(netid, oldPassword, newPassword);
        Console.WriteLine(changeResultMessage);

        return changeResultMessage;
        
    }
}
