/*
	Sections Page:
		Displays the current sections for a professor
		When a section is chosen it will look at the flag passed to determine which page to visit next 

		If clicking the back arrow, will return to the professor home page.

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/ 

using MauiApp1.ViewModels;
using MauiApp1.Models;
using MauiApp1.Pages;
using MauiApp1.Pages.ManageStudentPages;
using MauiApp1.Pages.ManageTeamsPages;

namespace MauiApp1.Pages;

public partial class Sections : ContentPage
{
	private string flag;
	private string professorID;
	private SectionsViewModel viewModel;
	public Sections(string netid, string flag){
		InitializeComponent();

		viewModel = new SectionsViewModel(netid);
		BindingContext = viewModel;
		this.flag = flag;
		professorID = netid;
		
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.InitializeASync(professorID);
	}

	// Takes the professor to the corresponding section's timesheets or peer reviews
	private async void OnClassSelected(object sender, SelectedItemChangedEventArgs e){
		var classPick = e.SelectedItem as Section;

		if(classPick == null)
		{
			return;
		}
		string secCode = classPick.code;
		
		// Navigating to the Timesheets Pages
		if (flag == "TIME"){
			await Navigation.PushAsync(new Timesheets(classPick.name, secCode));
		}

		// Navigating the the Peer Review Pages 
		if(flag == "PR"){
			await Navigation.PushAsync(new PeerReviewHome(professorID, secCode));
		}
		
		// Navigating to the Add students Pages 
		if(flag == "ADDSTU")
		{
			await Navigation.PushAsync(new ManageStudentPages.ManageStudents(secCode));
		}

		if(flag == "TEAM")
		{
			await Navigation.PushAsync(new ManageTeamsPages.ManageTeams(secCode));
		}
	}
}

