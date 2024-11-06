/*
	Team Reviews Page:
		NOT SURE WHAT THIS ONE DOES EITHER 

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...

*/

using MauiApp1.ViewModels;
namespace MauiApp1.Pages;


public partial class TeamReviews : ContentPage
{
	

	private ReviewViewModel viewModel;

	public TeamReviews(string className, string code)
	{
		InitializeComponent();
        viewModel = new ReviewViewModel(code);
		BindingContext = viewModel;
		SectionName.Text = className;
		

	}

	
}