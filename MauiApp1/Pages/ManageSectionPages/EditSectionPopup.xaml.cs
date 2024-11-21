/*
    Edit Section Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using System.Speech.Synthesis;
using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;
namespace MauiApp1.Pages.ManageSections;


public partial class EditSectionPopup : Popup
{
    string sectionCode;
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

    // If the changes want to be saved 
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
            // If no changes were made return the same section 
            Close(sectionPassed);
        }

        string updatedName = NameEntry.Text;
        string updatedCode = CodeEntry.Text;
        DateOnly updatedStart = DateOnly.Parse(StartDateEntry.Text);
        DateOnly updatedEnd = DateOnly.Parse(EndDateEntry.Text);

        List<string> updatedInfo = new List<string>{updatedName, updatedCode};
        List<DateOnly> updatedDates = new List<DateOnly>{updatedStart, updatedEnd};

        // Editing the section
        string editValidation = await viewModel.EditSectionAsync(sectionCode, updatedInfo, updatedDates);
        if(editValidation == "Success")
        {
            Close(new Section
            {
                name = updatedName,
                code = updatedCode, 
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
        Close(sectionPassed);
    }

}