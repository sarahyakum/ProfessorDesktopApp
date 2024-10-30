namespace MauiApp1.ViewModels;
using MauiApp1.Models;
public class LoginViewModel
{
    private DatabaseService databaseService;

    public LoginViewModel()
    {
        databaseService = new DatabaseService();
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        // Get the message from the stored procedure (success or error message)
        string loginResultMessage = await databaseService.Login(username, password);

        // Log the result (or use it in the UI)
        Console.WriteLine(loginResultMessage);

        return loginResultMessage;
    }

    public async Task<string> ChangePasswordAsync(string netid, string oldPassword, string newPassword){
        string changeResultMessage = await databaseService.changePassword(netid, oldPassword, newPassword);
        Console.WriteLine(changeResultMessage);

        return changeResultMessage;
        
    }
}
