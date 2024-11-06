/*
	Setting Page: MAYBE WE NEED TO CHANGE THIS NAME?
		NOT SURE WHAT THIS ONE DOES YET


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...


*/

namespace MauiApp1.Pages;

public partial class Settings : ContentPage
{
	string professorID;
	

	public Settings(string netid)
	{
		InitializeComponent();
		professorID = netid;
	}

	private async void OnCreatePeerReviewClicked(object sender, EventArgs e){
		string secCode = SectionCodeEntry.Text;
		string type = ReviewTypeEntry.Text;
		await DisplayAlert("Peer review by " + professorID + " for " + secCode, type, "OK");
	}
}
