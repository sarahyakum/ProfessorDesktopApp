/*
	Team Reviews Page:
		List of Teams per section and all the students and peer reviews within the team

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

*/

using MauiApp1.Models;
using MauiApp1.ViewModels;
namespace MauiApp1.Pages;
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
		//SectionName.Text = className;
		professor_id=netid;
		preview = pr;
		

	}

	private async void OnMemberClicked(object sender, SelectionChangedEventArgs e){
		var memberPick = e.CurrentSelection[0] as Student;
		if (memberPick == null){
			return;
		}
		await Navigation.PushAsync(new ViewScoresPopup(professor_id, preview, memberPick));
		//var popup = new ViewScoresPopup(professor_id, preview, memberPick);
		//await this.ShowPopupAsync(popup);


	}

	
}