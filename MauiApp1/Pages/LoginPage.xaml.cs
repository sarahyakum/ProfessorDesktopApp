namespace MauiApp1.Pages;

public partial class LoginPage : ContentPage
{
	

	public LoginPage()
	{
		InitializeComponent();
	}

	private void OnLoginButtonClicked(object sender, EventArgs e)
	{
		string netid = NetIDEntry.Text;
        string password = PasswordEntry.Text;
        if (netid == "abc12345" && password == "password")
            {
                DisplayAlert("Login", "Login Successful!", "OK");
            }
            else
            {
                DisplayAlert("Login", "Invalid username or password", "OK");
            }

	}
}

