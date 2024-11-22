/*
    Professor Home Page: 
        Prompts the professor to choose either the Time Tracking applications or the Peer Review applications.
        When the buttons are clicked, it will transition into the page for either appliccation.

        This page should be the landing page after the professor has logged in.



    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

*/

namespace MauiApp1.Pages;

public partial class HomePage : ContentPage

{
    private string id;
    private string flag;
    public HomePage(string netid)
    {
        InitializeComponent();
        id = netid;
    }

    // If the professor chooses to visit the Time Tracking 
    private async void OnTimesheetsButtonClicked(object sender, EventArgs e)
	{
		flag="TIME";
        await Navigation.PushAsync(new Sections(id, flag));
	}

    // If the professor chooses to visit the Peer Review
    private async void OnPeerReviewButtonClicked(object sender, EventArgs e)
    {
        flag="PR";
        await Navigation.PushAsync(new Sections(id, flag));
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Settings(id));

    }

    private async void OnEmailStudentsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EmailStudents(id));
    }
}