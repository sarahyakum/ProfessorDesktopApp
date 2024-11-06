/*
    Change Password Page:
        Allows the Professor to change their password. They are prompted to enter their username (NetID), old password, and new password.
        When they try to submit the change, it will be validated with the Database's stored procedures to ensure it meets the expected 
        input conditions.

        This page will automatically come up for the first login to the system when their password is their UTDID.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ...
        NetID: ....
*/


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


    // When they try to submit their changes, calls a procedure for the database to test whether their input is vaid or has violated the consitions
    private async void OnResetButtonClicked(object sender, EventArgs e){
        string usernameEntry = NetIDEntry.Text;
        string oldPasswordEntry = OldPasswordEntry.Text;
        newPassword = NewPasswordEntry.Text;
        string validation = await viewModel.ChangePasswordAsync(usernameEntry, oldPasswordEntry, newPassword);

        // Either accepts the change, or alerts the professor which condition was violated
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