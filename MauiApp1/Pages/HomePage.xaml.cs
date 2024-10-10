namespace MauiApp1.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }
    private async void OnTimesheetsButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("NewPage", "Proceed to Student Timesheets Page", "OK");

    }
    private async void OnPeerReviewButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("NewPage", "Proceed to Student Peer Review Page", "OK");

    }
}