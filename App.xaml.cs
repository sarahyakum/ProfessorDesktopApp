
namespace CS4485_Team75;


public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new NavigationPage(new Pages.Test());
		MainPage = new NavigationPage(new Pages.LoginPage());
		//MainPage = new NavigationPage(new Pages.TabsPage());
		//MainPage = new AppShell();
	}
}
