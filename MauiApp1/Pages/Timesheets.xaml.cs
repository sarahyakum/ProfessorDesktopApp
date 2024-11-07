
using MauiApp1.ViewModels;
using CommunityToolkit.Maui.Views;
using MauiApp1.Models;

namespace MauiApp1.Pages;


public partial class Timesheets : ContentPage
{
	public DateTime Timestamp { get; set; }

	private StudentSheetsViewModel viewModel;

	public Timesheets(string className, string code)
	{
		viewModel = new StudentSheetsViewModel(code);
		BindingContext = viewModel;
		InitializeComponent();
		DisplayTime();
		SectionName.Text = className;

		

	}

	private void DisplayTime(){
		
		Time.Text = DateTime.Now.ToString("G");
	}

	private async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e){
				// Inside any page
		var student = e.SelectedItem as Student;
		string netid = student.netid;


		var popup = new PopupPage(netid);
		this.ShowPopup(popup);
	}
}