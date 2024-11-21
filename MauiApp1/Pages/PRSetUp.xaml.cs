/*
	PR SET UP Page: 	NEEDS UPDATING
	Allows for professor to view current criteria as well as create new ones and create a peer review

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000


*/
using MauiApp1.ViewModels;
using MauiApp1.Models;
namespace MauiApp1.Pages;

public partial class PRSetUp : ContentPage
{
	string professorID;
	string section;
	string type;
	//List<string> criteriaInfo;
	private PRSetUpViewModel viewModel;

	public PRSetUp(string netid, string section)
	{
		InitializeComponent();
		professorID = netid;
		this.section = section;
		viewModel = new PRSetUpViewModel(netid, section);
		BindingContext = viewModel;
		SectionName.Text = section;
	}

	//Creates a new criteria category based on the input given
	private async void OnCriteriaClicked(object sender, EventArgs e){
		List<string> criteriaInfo = new List<string> { section, CriteriaEntry.Text, DescriptionEntry.Text, TypeEntry.Text };

		string criteriaValidation = await viewModel.CreateCriteriaAsync(professorID, criteriaInfo);
		if (criteriaValidation == "Success"){
			await DisplayAlert("New Criteria Added." , criteriaInfo[1], "OK");
		}
		else{
			await DisplayAlert("Error adding Criteria", criteriaValidation, "OK");
			
		}
	
	}

	//creates a new peer review for a section when the button is clicked
	private async void OnCreatePeerReviewClicked(object sender, EventArgs e){

		List<string> peerReviewInfo = new List<string>{ section, ReviewTypeEntry.Text} ;
		string start = TimePeriodEntry1.Text;
		string end = TimePeriodEntry2.Text;
		
		List<DateOnly> dates = new List<DateOnly>{DateOnly.Parse(start), DateOnly.Parse(end)};	


		string prValidation = await viewModel.PRAsync(professorID, peerReviewInfo, dates );
		if (prValidation == "Success"){
			await DisplayAlert("New Peer Review Created." ,  ReviewTypeEntry.Text , "OK");
		}
		else{
			await DisplayAlert("Error Creating Peer Review", prValidation, "OK");
			
		}
		
	}




}
