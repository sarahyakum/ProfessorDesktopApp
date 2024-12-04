/*
	PR SET UP Page: 
		Displays a list of the current criterion for the section 
		Allows the professor to add additional criteria
		Allows the professor to edit or delete criteria as long as they are not already being used in a peer review 
		Allows the professor to choose a criteria to reuse the name and description with a changed review type
		Displays the current peer reviews for the section 
		Allows the professor to add additional peer reviews 

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on November 1, 2024
        NetID: sny200000

	Worked on by Emma Hockett for CS 4485.0W1, Senior Design Project, Started on November 20, 2024
		NetID: ech210001

*/
using CS4485_Team75.ViewModels;
using CS4485_Team75.Models;
using CommunityToolkit.Maui.Views;
using CS4485_Team75.Pages.PRSetUp;
namespace CS4485_Team75.Pages.PRSetUpPages;

public partial class PRSetUp : ContentPage
{
	readonly string professorID;
	readonly string section;

	private readonly  PRSetUpViewModel viewModel;

	public PRSetUp(string netid, string sectionCode)
	{
		InitializeComponent();
		professorID = netid;
		section = sectionCode;
		viewModel = new PRSetUpViewModel(netid, section);
		BindingContext = viewModel;
	}


	//Creates a new criteria based on the input fields
	// Written by Sarah Yakum (sny200000)
	private async void OnCriteriaClicked(object sender, EventArgs e)
	{
		// Checing whether all of the fields are filled out 
		if(string.IsNullOrWhiteSpace(CriteriaEntry.Text) || string.IsNullOrWhiteSpace(DescriptionEntry.Text) || string.IsNullOrWhiteSpace(TypeEntry.Text) )
		{
			AddCriteriaErrorLabel.Text = "All fields must be filled out";
			return;
		}

		List<string> criteriaInfo = new List<string> { section, CriteriaEntry.Text, DescriptionEntry.Text, TypeEntry.Text };

		string criteriaValidation = await viewModel.CreateCriteriaAsync(professorID, criteriaInfo, section);
		if (criteriaValidation == "Success"){
			await DisplayAlert("New Criteria Added." , criteriaInfo[1], "OK");
		}
		else{
			AddCriteriaErrorLabel.Text = criteriaValidation;
			return;
		}
	}


	//Creates a new peer review for a section when the button is clicked
	// Written by Sarah Yakum (sny200000)
	private async void OnCreatePeerReviewClicked(object sender, EventArgs e)
	{
		// Checks whether all of the input fields are filled out 
		if(string.IsNullOrWhiteSpace(ReviewTypeEntry.Text) || string.IsNullOrWhiteSpace(TimePeriodEntry1.Text) || string.IsNullOrWhiteSpace(TimePeriodEntry2.Text))
		{
			AddPRErrorLabel.Text = "All fields must be filled out";
			return;
		}

		List<string> peerReviewInfo = new List<string>{ section, ReviewTypeEntry.Text} ;
		string start = TimePeriodEntry1.Text;
		string end = TimePeriodEntry2.Text;
		
		List<DateTime> dates = new List<DateTime>{DateTime.Parse(start), DateTime.Parse(end)};	

		// Confirms the professor is ready to create the peer review, knowing the consequences
		bool isConfirmed = await DisplayAlert("Create Peer Review", "Are you sure you want to create this peer review? Once it had been created you will not longer be able to: \n- Move students from teams\n- Remove teams\n- Edit criteria of this type\n- Delete criteria of this type\n- Change the Peer Review type", "OK", "Cancel");

		if(isConfirmed)
		{
			string prValidation = await viewModel.PRAsync(professorID, peerReviewInfo, dates, section);
			if (prValidation == "Success"){
				await DisplayAlert("New Peer Review Created." ,  ReviewTypeEntry.Text , "OK");
			}
			else{
				AddPRErrorLabel.Text = prValidation;
				return;
			}
		}
	}

	//Edits a criteria
	// Written by Emma Hockett (ech210001)
	private async void OnEditCriteriaClicked(object sender, EventArgs e){
		
		var criteria = (Criteria)((Button)sender).CommandParameter;

		string checkPRs = await viewModel.CheckCriteriaInPRAsync(section, criteria.reviewType);

		// If a peer review uses this criteria it cannot be edited 
		if(checkPRs == "Peer Review exists")
		{
			await DisplayAlert("Criteria Cannot Be Edited", "Criteria is already involved in a Peer Review, and cannot be edited", "OK");
			return;
		}

		var popup = new EditCriteriaPopup(viewModel, criteria);
		await this.ShowPopupAsync(popup);
	}

	// Deletes a criterion 
	// Written by Emma Hockett (ech210001)
	private async void OnDeleteCriteriaClicked(object sender, EventArgs e)
	{
		var criteria = (Criteria)((Button)sender).CommandParameter;

		// If a peer review exists with this criterion, then it cannot be deleted 
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


	// To reuse the criteria name and description with a different type 
	// Written by Emma Hockett (ech210001)
	private async void OnReuseCriteriaClicked(object sender, EventArgs e)
	{
		var criteria = (Criteria)((Button)sender).CommandParameter;
		var popup = new ReuseCriteriaPopup(viewModel, criteria, professorID);
		await this.ShowPopupAsync(popup);
	}


	// Allows the professor to alter the start date and end date of the Peer Reviews
	// Written by Emma Hockett (ech210001)
	private async void OnEditPeerReviewClicked(object sender, EventArgs e)
	{
		var peerReview = (PeerReview)((Button)sender).CommandParameter;
		var popup = new EditPRPopup(viewModel, peerReview);
		await this.ShowPopupAsync(popup);
	}

	// Allows the professor to delete a peer review 
	// Written by Emma Hockett (ech210001)
	private async void OnDeletePeerReviewClicked(object sender, EventArgs e)
	{
		var peerReview = (PeerReview)((Button)sender).CommandParameter;

		// Confirms whether the professot wants to delete the peer review and lists the consequences 
        bool isConfirmed = await DisplayAlert("Delete Peer Review", $"Are you sure you want to delete the {peerReview.type} Peer Review? All data including scores will be deleted as well.", "OK", "Cancel");

        if(isConfirmed)
        {
            string deleteValidation = await viewModel.DeletePRAsync(section, peerReview.type);

            if(deleteValidation == "Success")
            {
                await DisplayAlert("Peer Review Deleted", "Peer Review deleted successfully", "OK");
            }
            else{
                await DisplayAlert("Peer Review Not Deleted", "Peer Review not deleted", "OK");
            }
        }
        else{
            return;
        }
	}

}
