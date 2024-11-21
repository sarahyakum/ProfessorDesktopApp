/*
    Edit Peer Review Popup:
        Allows the professor to edit the start and end date of the peer reviews

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;
namespace MauiApp1.Pages.PRSetUp;


public partial class EditPRPopup : Popup
{
    string sectionCode;
    public PeerReview prPassed;
    private readonly PRSetUpViewModel viewModel;
    public EditPRPopup(PRSetUpViewModel viewModel, PeerReview pr)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        prPassed = pr;
        StartDateEntry.Text = pr.startDate.ToString("MM/dd/yyyy");
        EndDateEntry.Text = pr.endDate.ToString("MM/dd/yyyy");

        sectionCode = pr.section;
    }

    // If the changes want to be saved 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {
        // If the start or end date field is empty
        if(string.IsNullOrWhiteSpace(StartDateEntry.Text) || string.IsNullOrWhiteSpace(EndDateEntry.Text))
        {
            ErrorLabel.Text = "All fields must be filled out";
            return;
        }
        else if(prPassed.startDate == DateOnly.Parse(StartDateEntry.Text) && prPassed.endDate == DateOnly.Parse(EndDateEntry.Text))
        {
            // If nothing was changed
            Close(prPassed);
        }

        DateOnly updatedStart = DateOnly.Parse(StartDateEntry.Text);
        DateOnly updatedEnd = DateOnly.Parse(EndDateEntry.Text);

        string editValidation = await viewModel.EditPRDatesAsync(prPassed.section, prPassed.type, updatedStart, updatedEnd);

		if(editValidation == "Success")
		{
		    Close(new PeerReview
            {
                section = sectionCode,
                type = prPassed.type,
                startDate = updatedStart, 
                endDate = updatedEnd
            });
		}
        else{
            ErrorLabel.Text = editValidation;
            return;
        }
    }

    // If they choose their mind about editing, returns the same section 
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(prPassed);
    }

}