/*
    Professor Login Page:
        Automatically opens to this page when the application is started.
        Prompts the professor to enter their username (NetID) and password. 
        
        Calls a procedure to check the credentials against the databse:
            Correct Login: Takes the user to their home page
            Correct Login and Password = UTDID: Takes the user to the change password page
            Incorrect Login: Tells the user their login information was incorrect and remains on this page


    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000


*/

using System.Runtime.Serialization;
using MauiApp1.ViewModels;

namespace MauiApp1.Pages;


public partial class LoginPage : ContentPage
{
	private readonly LoginViewModel viewModel;
    private readonly Professor professor;

	public LoginPage()
	{
		InitializeComponent();
        viewModel = new LoginViewModel();
        professor = new Professor();
        
	}


    // When the professor tries to login to their account
	private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
        // Checks whether all of the fields have been filled out 
        if(string.IsNullOrWhiteSpace(NetIDEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            LoginErrorLabel.Text = "All fields must be filled out";
            return;
        }

		string netid = NetIDEntry.Text;
        string password = PasswordEntry.Text;

        string loginValidation = await viewModel.LoginAsync(netid, password);

        // Determing which page to direct the user to based on the feedback from the procedure
        if (loginValidation == "Success")
        {
            await Navigation.PushAsync(new HomePage(netid));
            professor.username = netid;
            professor.password = password;

        }
        else if(loginValidation == "Change password")
        {
            // Upon first time logins the professor must change their password
            await Navigation.PushAsync(new ChangePassword(netid));
        }
        else
        {
            LoginErrorLabel.Text = loginValidation;
            return;
        }
	}

}

