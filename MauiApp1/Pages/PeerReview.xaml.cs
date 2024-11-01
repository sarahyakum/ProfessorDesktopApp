namespace MauiApp1.Pages;

public partial class PeerReview : ContentPage
{
	
	private string professorID;
	public PeerReview(string netid)
	{
		InitializeComponent();
		professorID = netid;
	}

	private async void OnViewButtonClicked(object sender, EventArgs e){
		await Navigation.PushAsync(new Sections(professorID));

	}
	private async void OnSetUpButtonClicked(object sender, EventArgs e){
		await Navigation.PushAsync(new Settings(professorID));
		
	}

}

