/*
	Team Reviews Page:
		List of Teams per section and all the students and peer reviews within the team

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on November 1, 2024
        NetID: sny200000

*/

using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
namespace CS4485_Team75.Pages;
using CommunityToolkit.Maui.Views;


public partial class TeamReviews : ContentPage
{
	string professor_id;

	private ReviewViewModel viewModel;

	public PeerReview preview;
	public TeamReviews(string netid, string code, PeerReview pr)
	{
		InitializeComponent();
        viewModel = new ReviewViewModel(code);
		BindingContext = viewModel;
		professor_id=netid;
		preview = pr;
		

	}

	// Navigates to individuals scores from team members in the team selected
	private async void OnMemberClicked(object sender, SelectionChangedEventArgs e){
		var memberPick = e.CurrentSelection[0] as Student;
		if (memberPick == null){
			return;
		}
		Student stu = memberPick;
		await Navigation.PushAsync(new ViewScores(professor_id, preview, stu));
		


	}

	
}