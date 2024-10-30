using MauiApp1.ViewModels;

namespace MauiApp1.Pages;

public partial class ChangePassword : ContentPage{
    private string id;
    private string oldPassword;
    private string newPassword;
    LoginViewModel viewModel;

    public ChangePassword(string netid, string password){

        

        InitializeComponent();
        viewModel = new LoginViewModel();
        id = netid;
        oldPassword = password;
        
    }

    private async void OnResetButtonClicked(object sender, EventArgs e){
        string usernameEntry = NetIDEntry.Text;
        string oldPasswordEntry = OldPasswordEntry.Text;
        newPassword = NewPasswordEntry.Text;
        string validation = await viewModel.ChangePasswordAsync(usernameEntry, oldPasswordEntry, newPassword);

        if(validation == "Success"){

            await DisplayAlert(validation, "Password was changed.", "OK");
            await Navigation.PushAsync(new HomePage(id));
        }
        else if (validation == "Incorrect username or password"){
            await DisplayAlert("Reenter Form", validation, "OK");

        }else if(validation == "Password cannot be the same"){
            await DisplayAlert("Change New Password", validation, "OK");

        }else if(validation=="Password cannot be UTD ID"){
            await DisplayAlert("Change New Password", validation, "OK");
        }
        else{
            await DisplayAlert("Database Issue", validation, "OK");
        }

        

        
    }



}