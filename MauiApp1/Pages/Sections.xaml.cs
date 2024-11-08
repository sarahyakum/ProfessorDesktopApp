/*
	Sections Page:
		Promts the professor to choose which section they would like the view the timesheets for.
		Upon choosing which section, taken to the page for that section to display the student's timesheets.

		If clicking the back arrow, will return to the professor home page.

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/ 

using MauiApp1.ViewModels;
using MauiApp1.Models;

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

	// Takes the professor to the corresponding section's timesheets or peer reviews
	private async void OnClassSelected(object sender, SelectedItemChangedEventArgs e){
		var classPick = e.SelectedItem as Section;
		string secCode = classPick.code;
		
		if (flag == "TIME"){

			await Navigation.PushAsync(new Timesheets(classPick.name, secCode));

		}

		if(flag == "PR"){
			await Navigation.PushAsync(new PeerReview(professorID, secCode));
		}
		

	}

	
	
	
}

