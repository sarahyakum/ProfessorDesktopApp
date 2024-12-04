/*
	Review Type List:
        Navigates the professor through the different review types they have
        in their section

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on December 1, 2024
        NetID: sny200000
*/ 

using CS4485_Team75.ViewModels;
using CS4485_Team75.Models;
using CS4485_Team75.Pages;
using CS4485_Team75.Pages.ManageStudentPages;
using CS4485_Team75.Pages.ManageTeamsPages;

namespace CS4485_Team75.Pages;

public partial class ReviewTypeList : ContentPage
{

    public ReviewViewModel viewModel;
    string id;
    string section;
   
	
	public ReviewTypeList(string netid, string code){
		InitializeComponent();
        viewModel = new ReviewViewModel(code);
        BindingContext = viewModel;
        id = netid;
        section = code;
		
	}

    // Sends professor to view the reviews by teams based on selection
    private async void OnReviewTypeClicked(object sender, SelectedItemChangedEventArgs e){
        var reviewPick = e.SelectedItem as PeerReview;
        if(reviewPick == null)
		{
			return;
		}

        await Navigation.PushAsync(new TeamReviews(id, section, reviewPick));
    }


}

