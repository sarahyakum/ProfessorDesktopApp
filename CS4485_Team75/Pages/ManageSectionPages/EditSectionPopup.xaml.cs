/*
    Edit Section Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered
        Allows to save the changes or cancel and close the popup

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
namespace CS4485_Team75.Pages.ManageSections;


public partial class EditSectionPopup : Popup
{
    readonly string sectionCode;
    public Section sectionPassed;
    private readonly ManageSectionsViewModel viewModel;
    public EditSectionPopup(ManageSectionsViewModel viewModel, Section section)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        sectionPassed = section;
        NameEntry.Text = section.name;
        CodeEntry.Text = section.code;
        StartDateEntry.Text = section.startDate.ToString("MM/dd/yyyy");
        EndDateEntry.Text = section.endDate.ToString("MM/dd/yyyy");

        sectionCode = section.code;
    }

    // If the professor tries to save the changes that were made 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {

        // Checks whether all of the fields are filled out 
        if(string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(CodeEntry.Text) || string.IsNullOrWhiteSpace(StartDateEntry.Text) || string.IsNullOrWhiteSpace(EndDateEntry.Text))
        {
            ErrorLabel.Text = "All fields must be filled out to save changes.";
            return;
        }
        else if(sectionPassed.name == NameEntry.Text && sectionPassed.code == CodeEntry.Text && sectionPassed.startDate == DateOnly.Parse(StartDateEntry.Text) && sectionPassed.endDate == DateOnly.Parse(EndDateEntry.Text) )
        {
            // If no changes were made close the popup
            Close();
        }

        string updatedName = NameEntry.Text;
        string updatedCode = CodeEntry.Text;
        DateOnly updatedStart = DateOnly.Parse(StartDateEntry.Text);
        DateOnly updatedEnd = DateOnly.Parse(EndDateEntry.Text);

        List<string> updatedInfo = new List<string>{updatedName, updatedCode};
        List<DateOnly> updatedDates = new List<DateOnly>{updatedStart, updatedEnd};

        // Editing the section by calling the viewModel method for editing a section 
        string editValidation = await viewModel.EditSectionAsync(sectionCode, updatedInfo, updatedDates);
        if(editValidation == "Success")
        {
            Close();
        }
        else{
            // If the changes were not able to be made, displays the reason why
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