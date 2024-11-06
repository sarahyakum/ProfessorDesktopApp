/*
	Sections Page:
		Prompts the professor which section he would like to view the information for.

		IS THIS FOR THE PEER REVIEWS OR THE TIME TRACKING OR BOTH?


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...
*/

namespace MauiApp1.Pages;
using MauiApp1.ViewModels;
using MauiApp1.Models;

public partial class Sections : ContentPage
{
	private TimeTrackViewModel viewModel;

	public Sections(string netid)
	{
		InitializeComponent();
		viewModel = new TimeTrackViewModel(netid);
		BindingContext = viewModel;
	}

	private async void OnClassSelected(object sender, SelectedItemChangedEventArgs e){
		var classPick = e.SelectedItem as Section;
		string secCode = classPick.code;
		
		await Navigation.PushAsync(new TeamReviews(classPick.name, secCode));
	}

	

}

