/*
    LoginViewModel Class
        Handles the authentication between the user interface and the database for the professor login
        and changing password

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on October 24, 2024
        NETID: sny200000

*/

namespace CS4485_Team75.ViewModels;
using CS4485_Team75.Models;
public class LoginViewModel
{
    private readonly DatabaseService databaseService;

    public LoginViewModel()
    {
        databaseService = new DatabaseService();
    }

    // Calls the method in the Database.cs model to check whether the given information is a correct login and returns it to the page 
    public async Task<string> LoginAsync(string username, string password)
    {
        string loginResultMessage = await databaseService.Login(username, password);
        return loginResultMessage;
    }

    // Calls the method to change the password for the professor given they need to
    public async Task<string> ChangePasswordAsync(string netid, string oldPassword, string newPassword)
    {
        string changeResultMessage = await databaseService.ChangePassword(netid, oldPassword, newPassword);
        return changeResultMessage;
    }
}
