/*
	Team Reviews Page:
		List of Teams per section and all the students and peer reviews within the team

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

*/

using MauiApp1.ViewModels;
namespace MauiApp1.Pages;


public partial class TeamReviews : ContentPage
{
	

	private ReviewViewModel viewModel;

	public TeamReviews(string netid, string code)
	{
		InitializeComponent();
        viewModel = new ReviewViewModel(code);
		BindingContext = viewModel;
		//SectionName.Text = className;
		

	}

	
}