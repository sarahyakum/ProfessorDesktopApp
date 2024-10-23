

namespace MauiApp1.Pages;
using MauiApp1.ViewModels;

public partial class Timesheets : ContentPage
{
	public DateTime Timestamp { get; set; }

	public Timesheets(string className)
	{
		InitializeComponent();

		DisplayTime();

		BindingContext = new StudentHoursViewModel();

	}

	private void DisplayTime(){
		Time.Text = DateTime.Now.ToString("G");
	}
}