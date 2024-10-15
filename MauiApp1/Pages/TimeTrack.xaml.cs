

namespace MauiApp1.Pages;

public partial class TimeTrack : ContentPage
{
	 

	public TimeTrack()
	{
		InitializeComponent();

		List<string> classes = new List<string>(){
			"CS4485", "CS4365", "CS4352"

		};
		listClasses.ItemsSource = classes;
		
	}

	private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e){
		String className = (string)e.SelectedItem;
		
		if (className=="CS4485"){ 
			await Navigation.PushAsync(new Timesheets());
		}
		//return className;
	}
	

	
	
	
	
	
}

