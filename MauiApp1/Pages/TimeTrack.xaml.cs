using MauiApp1.ViewModels;
using MauiApp1.Models;

namespace MauiApp1.Pages;

public partial class TimeTrack : ContentPage
{
	private TimeTrackViewModel viewModel;
	public TimeTrack(string netid){
		InitializeComponent();

		viewModel = new TimeTrackViewModel(netid);
		BindingContext = viewModel;
		

	}


	private async void OnClassSelected(object sender, SelectedItemChangedEventArgs e){
		var classPick = e.SelectedItem as Section;
		
		await Navigation.PushAsync(new Timesheets(classPick.name));

	}
	
}

