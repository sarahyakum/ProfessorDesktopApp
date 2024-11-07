/*
	TimeTrack Page:
		Promts the professor to choose which section they would like the view the timesheets for.
		Upon choosing which section, taken to the page for that section to display the student's timesheets.

		If clicking the back arrow, will return to the professor home page.

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...
*/ 

using MauiApp1.ViewModels;
using MauiApp1.Models;

namespace MauiApp1.Pages;

public partial class TimeTrack : ContentPage
{
	private TimeTrackViewModel viewModel;
	public TimeTrack(string netid){
		InitializeComponent();

		viewModel = new TimeTrackViewModel(netid);
		BindingContext = viewModel;
		

	}

	// Takes the professor to the corresponding section's timesheets
	private async void OnClassSelected(object sender, SelectedItemChangedEventArgs e){
		var classPick = e.SelectedItem as Section;
		string secCode = classPick.code;
		
		await Navigation.PushAsync(new Timesheets(classPick.name, secCode));

	}

	
	
	
}

