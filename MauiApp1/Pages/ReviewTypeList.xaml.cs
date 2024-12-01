/*
	Review Type List:
        Navigates the professor through the different review types they have
        in their section

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000
*/ 

using MauiApp1.ViewModels;
using MauiApp1.Models;
using MauiApp1.Pages;
using MauiApp1.Pages.ManageStudentPages;
using MauiApp1.Pages.ManageTeamsPages;

namespace MauiApp1.Pages;

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

    private async void OnReviewTypeClicked(object sender, SelectedItemChangedEventArgs e){
        var reviewPick = e.SelectedItem as PeerReview;
        if(reviewPick == null)
		{
			return;
		}

        await Navigation.PushAsync(new TeamReviews(id, section, reviewPick));
    }


}

