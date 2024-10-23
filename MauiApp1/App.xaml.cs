
namespace MauiApp1;


public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new NavigationPage(new Pages.Timesheets());
		MainPage = new NavigationPage(new Pages.LoginPage());
		//MainPage = new NavigationPage(new Pages.TabsPage());
		//MainPage = new AppShell();
	}
}
