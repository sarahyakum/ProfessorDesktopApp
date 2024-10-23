using MauiApp1.Models;

namespace MauiApp1.Pages;

public partial class TimeTrack : ContentPage
{
	
	public TimeTrack(){
		InitializeComponent();

		var classes = new List<Class>{
			new Class{Name = "CS 4485"},
			new Class{Name = "CS 1234"},
			new Class{Name = "CS 2424"},
			new Class{Name = "CS 5678"},
			new Class{Name = "CS 1212"},
			new Class{Name = "CS 9876"}
		};

		classList.ItemsSource = classes;
		
		



	}

	private async void OnClassSelected(object sender, SelectedItemChangedEventArgs e){
		var classPick = e.SelectedItem as Class;
		
		await Navigation.PushAsync(new Timesheets(classPick.Name));

	}
	
}

