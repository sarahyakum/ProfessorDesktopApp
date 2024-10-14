namespace MauiApp1.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }
    private async void OnTimesheetsButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TimeTrack());
        

	}
    private async void OnPeerReviewButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PeerReview());

    }
}