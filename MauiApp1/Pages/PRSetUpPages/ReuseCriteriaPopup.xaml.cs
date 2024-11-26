/*
    Reuse Criteria Popup:
        Allows the professor to select a criteria from a previous review type and add it with a new type

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;
namespace MauiApp1.Pages.PRSetUp;


public partial class ReuseCriteriaPopup : Popup
{
    readonly string sectionCode;
    readonly string professorID;
    public Criteria criteriaPassed;
    private readonly PRSetUpViewModel viewModel;
    public ReuseCriteriaPopup(PRSetUpViewModel viewModel, Criteria criteria, string netid)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        criteriaPassed = criteria;
        TypeEntry.Text = criteria.reviewType;
        professorID = netid;
        sectionCode = criteria.section;
    }

    // If the changes want to be saved 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {
        // If the new review type has not been filled out 
        if(string.IsNullOrWhiteSpace(TypeEntry.Text))
        {
            ErrorLabel.Text = "New Type must be filled out";
            return;
        }
        else if (criteriaPassed.reviewType == TypeEntry.Text)
        {
            // If the review type was not changed 
            Close();
        }

        string updatedType = TypeEntry.Text;

        List<string> criteriaInfo = new List<string> { sectionCode, criteriaPassed.name, criteriaPassed.description ?? string.Empty, updatedType };

        // Calls the viewmodel to perform the actions to reuse the criteria and exits pop up or tells what it wrong
		string criteriaValidation = await viewModel.CreateCriteriaAsync(professorID, criteriaInfo, sectionCode);
		if (criteriaValidation == "Success"){
			Close();
		}
		else{
			ErrorLabel.Text = criteriaValidation;
            return;
		}
    }

    // If they choose their mind about editing, closes the popup
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

}