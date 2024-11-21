/*
    Edit Peer Review Popup:
        Allows the professor to edit the start and end date of the peer reviews

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.PRSetUp;


public partial class EditPRPopup : Popup
{
    string sectionCode;
    public PeerReview prPassed;
    public EditPRPopup(PeerReview pr)
    {
        InitializeComponent();
        prPassed = pr;
        StartDateEntry.Text = pr.startDate.ToString("MM/dd/yyyy");
        EndDateEntry.Text = pr.endDate.ToString("MM/dd/yyyy");

        sectionCode = pr.section;
    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        DateOnly updatedStart = DateOnly.Parse(StartDateEntry.Text);
        DateOnly updatedEnd = DateOnly.Parse(EndDateEntry.Text);

        Close(new PeerReview
        {
            section = sectionCode,
            type = prPassed.type,
            startDate = updatedStart, 
            endDate = updatedEnd
        });
    }

    // If they choose their mind about editing, returns the same section 
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(prPassed);
    }

}