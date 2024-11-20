/*
	PR SET UP Page: 	NEEDS UPDATING
	Allows for professor to view current criteria as well as create new ones and create a peer review

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000

	Worked on by Emma Hockett for CS 4485.0W1, Senior Design Project, Started on November 20, 2024
		NetID: ech210001

*/
using MauiApp1.ViewModels;
using MauiApp1.Models;
using CommunityToolkit.Maui.Views;
namespace MauiApp1.Pages.PRSetUpPages;

public partial class PRSetUp : ContentPage
{
	string professorID;
	string section;
	string type;
	//List<string> criteriaInfo;
	private PRSetUpViewModel viewModel;

	public PRSetUp(string netid, string sectionCode)
	{
		InitializeComponent();
		professorID = netid;
		section = sectionCode;
		this.section = sectionCode;
		viewModel = new PRSetUpViewModel(netid, section);
		BindingContext = viewModel;
	}

	//Creates a new criteria category based on the input given
	private async void OnCriteriaClicked(object sender, EventArgs e){
		List<string> criteriaInfo = new List<string> { section, CriteriaEntry.Text, DescriptionEntry.Text, TypeEntry.Text };

		string criteriaValidation = await viewModel.CreateCriteriaAsync(professorID, criteriaInfo, section);
		if (criteriaValidation == "Success"){
			await DisplayAlert("New Criteria Added." , criteriaInfo[1], "OK");
		}
		else{
			await DisplayAlert("Error adding Criteria", criteriaValidation, "OK");
			
		}
	}

	//Creates a new peer review for a section when the button is clicked
	private async void OnCreatePeerReviewClicked(object sender, EventArgs e){

		List<string> peerReviewInfo = new List<string>{ section, ReviewTypeEntry.Text} ;
		string start = TimePeriodEntry1.Text;
		string end = TimePeriodEntry2.Text;
		
		List<DateTime> dates = new List<DateTime>{DateTime.Parse(start), DateTime.Parse(end)};	


		// Confirms the professor is ready to create the peer review, knowing the consequences
		bool isConfirmed = await DisplayAlert("Create Peer Review", "Are you sure you want to create this peer review? Once it had been created you will not longer be able to: move students from teams, remove teams, edit criteria of this type, delete criteris of this type.", "OK", "Cancel");

		if(isConfirmed)
		{
			string prValidation = await viewModel.PRAsync(professorID, peerReviewInfo, dates, section);
			if (prValidation == "Success"){
				await DisplayAlert("New Peer Review Created." ,  ReviewTypeEntry.Text , "OK");
			}
			else{
				await DisplayAlert("Error Creating Peer Review", prValidation, "OK");
			}
		}
	}

	//Edits a criteria
	// Written by Emma Hockett (ech210001), Started on November 20, 2024
	private async void OnEditCriteriaClicked(object sender, EventArgs e){
		
		var criteria = (Criteria)((Button)sender).CommandParameter;
		string sectionCode = criteria.section;

		string checkPRs = await viewModel.CheckCriteriaInPRAsync(section, criteria.reviewType);

		if(checkPRs == "Peer Review exists")
		{
			await DisplayAlert("Criteria Cannot Be Edited", "Criteria is already involved in a Peer Review, and cannot be edited", "OK");
			return;
		}

		var popup = new EditCriteriaPopup(criteria);
		var result = await this.ShowPopupAsync(popup) as Criteria;

		// If no changes were made or the user chose to cancel
		if(criteria.name == result.name && criteria.description == result.description && criteria.reviewType == result.reviewType)
		{
			return;
		}

		// Edit the criteria and return the result message
		string editValidation = await viewModel.EditCriteriaAsync(section, criteria, result);

		if(editValidation == "Success")
		{
			await DisplayAlert("Criteria Edited", "Criteria edited successfully", "OK");
		}
		else{
			await DisplayAlert("Criteria Not Altered", editValidation, "OK");
		}
	}

	// Deletes a criterion 
	// Written by Emma Hockett (ech210001), Started on November 20, 2024
	private async void OnDeleteCriteriaClicked(object sender, EventArgs e)
	{
		var criteria = (Criteria)((Button)sender).CommandParameter;
		string sectionCode = criteria.section;

		string checkPRs = await viewModel.CheckCriteriaInPRAsync(section, criteria.reviewType);

		if(checkPRs == "Peer Review exists")
		{
			await DisplayAlert("Criteria Cannot Be Deleted", "Criteria is already involved in a Peer Review, and cannot be deleted", "OK");
			return;
		}

		// Checks whether they are sure if they want to delete the criteria
		bool isConfirmed = await DisplayAlert("Delete Section", $"Are you sure you want to delete {criteria.name}?", "OK", "Cancel");
		if(isConfirmed)
		{
			string deleteValidation = await viewModel.DeleteCriteriaAsync(section, criteria.name, criteria.reviewType);

			if(deleteValidation == "Success")
			{
				await DisplayAlert("Criterion Deleted", "Criterion was deleted successfully", "OK");
			}
			else{
				await DisplayAlert("Criterion Not Deleted", deleteValidation, "OK");
			}
		}
	}

}
