/*
    Add Section Page:
        Allows the professor to see their current sections 
        Based on their current sections they can edit or delete them 
        They can also add new section 

    Written by Emma Hockett for CS 4485.0W1, Started on November 15, 2024
        NetID: ech210001
*/

using MauiApp1.ViewModels;
using MauiApp1.Models;
using MauiApp1.Pages.ManageSections;
using CommunityToolkit.Maui.Views;
namespace MauiApp1.Pages.ManageSectionPages;

public partial class ManageSections : ContentPage
{
    string professorID;

    private ManageSectionsViewModel viewModel;

    public ManageSections(string netid)
    {
        InitializeComponent();
        professorID = netid;
        viewModel = new ManageSectionsViewModel(netid);
        BindingContext = viewModel;
    }

    // For adding a section from the text input boxes on the page 
    private async void OnAddSectionClicked(object sender, EventArgs e)
    {

        // Checks whether all of the fields are filled out and displays a message if they are not
        if(string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(CodeEntry.Text) || string.IsNullOrWhiteSpace(StartEntry.Text) || string.IsNullOrWhiteSpace(EndEntry.Text))
        {
            AddSectionErrorLabel.Text = "All fields must be filled out to add a section.";
            return;
        }

        List<string> sectionInfo = new List<string> {NameEntry.Text, CodeEntry.Text};
        string start = StartEntry.Text;
        string end = EndEntry.Text;
        List<DateOnly> dates = new List<DateOnly>{DateOnly.Parse(start), DateOnly.Parse(end)};

        // Connecting to the database through the view model to add the section
        string sectionValidation = await viewModel.AddSectionAsync(professorID, sectionInfo, dates);
        if(sectionValidation == "Success"){
            await DisplayAlert("New Section Added.", sectionInfo[0], "OK");
        }
        else{
            AddSectionErrorLabel.Text = sectionValidation;
            return;
        }
    }

    // For editing the section, all of the logic is taken care of in the popup
    private async void OnEditSectionClicked(object sender, EventArgs e)
    {
        // Pulling the section that is being edited and then creating the popup
        var section = (Section)((Button)sender).CommandParameter;
        string sectionCode = section.code;
        var popup = new EditSectionPopup(viewModel, section);
        var result = await this.ShowPopupAsync(popup) as Section;
    }

    // Allows the professor to delete a section 
    private async void OnDeleteSectionClicked(object sender, EventArgs e)
    {
        var section = (Section)((Button)sender).CommandParameter;
        string sectionCode = section.code;

        bool isConfirmed = await DisplayAlert("Delete Section", $"Are you sure you want to delete section {sectionCode}? All data including students, teams, timeslots, and reviews will be removed.", "OK", "Cancel");

        if(isConfirmed)
        {
            string deleteValidation = await viewModel.DeleteSectionAsync(sectionCode);

            if(deleteValidation == "Success")
            {
                await DisplayAlert("Section Deleted", "Section deleted successfully", "OK");
            }
            else{
                await DisplayAlert("Section Not Deleted", "Section not deleted", "OK");
            }
        }
        else{
            return;
        }
    }
}