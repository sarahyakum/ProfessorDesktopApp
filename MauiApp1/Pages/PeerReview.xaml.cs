/*
	Peer Review Page: 
		Prompts the professor to choose whether they would like to view the already created Peer Reviews and scores, or work on creating a new one.
		Takes the professor to the corresponding page to the button they click.
		The back arrow returns the professor to their home page

	
	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/

namespace MauiApp1.Pages;

public partial class PeerReview : ContentPage
{
	
	private string professorID;
	private string secCode;
	public PeerReview(string netid, string section)
	{
		InitializeComponent();
		professorID = netid;
		secCode = section;
	}

	// Takes the professor to a page to choose between their sections 
	private async void OnViewButtonClicked(object sender, EventArgs e){
		await Navigation.PushAsync(new TeamReviews(professorID, secCode));

	}

	// Takes the professor to a page to allow them to enter new Peer Review Information
	private async void OnSetUpButtonClicked(object sender, EventArgs e){
		await Navigation.PushAsync(new Settings(professorID));
		
	}

}

