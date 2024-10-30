using System.Runtime.Serialization;
using MauiApp1.ViewModels;

namespace MauiApp1.Pages;


public partial class LoginPage : ContentPage
{
	private LoginViewModel viewModel;
    private Professor professor;

	public LoginPage()
	{
		InitializeComponent();
        viewModel = new LoginViewModel();
        
	}


	private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
		string netid = NetIDEntry.Text;
        string password = PasswordEntry.Text;

        string loginValidation = await viewModel.LoginAsync(netid, password);

        if (loginValidation == "Success")
        {

            await Navigation.PushAsync(new HomePage(netid));
            professor.username = netid;
            professor.password = password;

            //DisplayAlert("Login", "Login Successful!", "OK");
        }
        else if(loginValidation == "Change password"){
            await Navigation.PushAsync(new ChangePassword(netid, password));
        }
        else
        {
            await DisplayAlert("Login Error", loginValidation, "OK");
        }

	}

    
    

}

