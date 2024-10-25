namespace MauiApp1.Pages;

public partial class HomePage : ContentPage

{
    private string id;
    public HomePage(string netid)
    {
        InitializeComponent();
        id = netid;
    }
    private async void OnTimesheetsButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TimeTrack(id));
        

	}
    private async void OnPeerReviewButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PeerReview(id));

    }
}