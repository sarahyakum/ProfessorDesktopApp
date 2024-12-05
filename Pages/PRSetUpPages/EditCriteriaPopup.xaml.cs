/*
    Edit Criteria Popup:
        Displays the current information about the criteria and allows the professor to edit it

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
namespace CS4485_Team75.Pages.PRSetUpPages;


public partial class EditCriteriaPopup : Popup
{
    readonly string sectionCode;
    public Criteria criteriaPassed;
    private readonly PRSetUpViewModel viewModel;
    public EditCriteriaPopup(PRSetUpViewModel viewModel, Criteria criteria)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        criteriaPassed = criteria;
        NameEntry.Text = criteria.name;
        DescriptionEntry.Text = criteria.description;
        TypeEntry.Text = criteria.reviewType;

        sectionCode = criteria.section;
    }

    // If the changes want to be saved 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {
        // Checks whether all of the fields were filled out 
        if(string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(DescriptionEntry.Text) || string.IsNullOrWhiteSpace(TypeEntry.Text))
        {
            ErrorLabel.Text = "All fields must be filled out";
        }
        else if(criteriaPassed.name == NameEntry.Text && criteriaPassed.description == DescriptionEntry.Text && criteriaPassed.reviewType == TypeEntry.Text)
        {
            // If no changes were made 
            Close();
        }

        string updatedName = NameEntry.Text;
        string updatedDescription = DescriptionEntry.Text;
        string updatedType = TypeEntry.Text;

        Criteria updatedCriteria = new Criteria {name = updatedName, description = updatedDescription, reviewType = updatedType, section = sectionCode};

		// Edit the criteria and return the result message
		string editValidation = await viewModel.EditCriteriaAsync(sectionCode, criteriaPassed, updatedCriteria);

		if(editValidation == "Success")
		{
			Close();
		}
		else{
			ErrorLabel.Text = editValidation;
            return;
		}
    }

    // If they choose their mind about editing, closes the popup
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

}