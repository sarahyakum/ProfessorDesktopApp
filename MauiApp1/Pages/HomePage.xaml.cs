/*
    Professor Home Page: 
        Prompts the professor to choose either the Time Tracking applications or the Peer Review applications.
        When the buttons are clicked, it will transition into the page for either appliccation.

        This page should be the landing page after the professor has logged in.

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...

*/

namespace MauiApp1.Pages;

public partial class HomePage : ContentPage

{
    private string id;
    public HomePage(string netid)
    {
        InitializeComponent();
        id = netid;
    }

    // If the professor chooses to visit the Time Tracking 
    private async void OnTimesheetsButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TimeTrack(id));
        

	}

    // If the professor chooses to visit the Peer Review
    private async void OnPeerReviewButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PeerReview(id));

    }
}