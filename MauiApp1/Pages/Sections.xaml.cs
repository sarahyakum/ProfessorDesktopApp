namespace MauiApp1.Pages;
using MauiApp1.ViewModels;

public partial class Sections : ContentPage
{
	private TimeTrackViewModel viewModel;

	public Sections(string netid)
	{
		InitializeComponent();
		viewModel = new TimeTrackViewModel(netid);
		BindingContext = viewModel;
	}

	private async void OnClassSelected(object sender, EventArgs e){
		await DisplayAlert("section", "you chose section", "ok");
	}

	

}

