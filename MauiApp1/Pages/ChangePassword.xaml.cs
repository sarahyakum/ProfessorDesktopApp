/*
    Change Password Page:
        Allows the Professor to change their password. They are prompted to enter their username (NetID), old password, and new password.
        When they try to submit the change, it will be validated with the Database's stored procedures to ensure it meets the expected 
        input conditions.

        This page will automatically come up for the first login to the system when their password is their UTDID.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ...
        NetID: sny200000
*/

using MauiApp1.ViewModels;
namespace MauiApp1.Pages;

public partial class ChangePassword : ContentPage{
    private readonly string id;
    private readonly LoginViewModel viewModel;

    public ChangePassword(string netid)
    {
        InitializeComponent();
        viewModel = new LoginViewModel();
        id = netid;
    }


    // When they try to submit their changes, calls a procedure for the database to test whether their input is vaid or has violated the constraints
    private async void OnResetButtonClicked(object sender, EventArgs e)
    {
        // Makes sure all fields are filled in
        if(string.IsNullOrWhiteSpace(NetIDEntry.Text) || string.IsNullOrWhiteSpace(OldPasswordEntry.Text) || string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
        {
            ChangePasswordErrorLabel.Text = "All fields must be filled out.";
            return;
        }

        string usernameEntry = NetIDEntry.Text;
        string oldPasswordEntry = OldPasswordEntry.Text;
        string newPassword = NewPasswordEntry.Text;

        string validation = await viewModel.ChangePasswordAsync(usernameEntry, oldPasswordEntry, newPassword);

        // Either accepts the change, or alerts the professor which condition was violated
        if(validation == "Success")
        {
            await DisplayAlert(validation, "Password was changed.", "OK");
            await Navigation.PushAsync(new HomePage(id));
        }
        else
        {
            ChangePasswordErrorLabel.Text = validation;
            return;
        }
    }
}