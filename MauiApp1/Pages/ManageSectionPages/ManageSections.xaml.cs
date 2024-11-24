/*
    Add Section Page:
        Allows the professor to see their current sections 
        Based on their current sections they can edit or delete them 
        They can also add new sections 

    Written entirely by Emma Hockett for CS 4485.0W1, Started on November 15, 2024
        NetID: ech210001
*/

using MauiApp1.ViewModels;
using MauiApp1.Models;
using MauiApp1.Pages.ManageSections;
using CommunityToolkit.Maui.Views;
namespace MauiApp1.Pages.ManageSectionPages;

public partial class ManageSections : ContentPage
{
    readonly string professorID;

    private ManageSectionsViewModel viewModel;

    public ManageSections(string netid)
    {
        InitializeComponent();
        professorID = netid;
        viewModel = new ManageSectionsViewModel(netid);
        BindingContext = viewModel;
    }


    // When the professor tries to add a section from the input fields
    private async void OnAddSectionClicked(object sender, EventArgs e)
    {

        // Checks whether all of the fields are filled out
        if(string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(CodeEntry.Text) || string.IsNullOrWhiteSpace(StartEntry.Text) || string.IsNullOrWhiteSpace(EndEntry.Text))
        {
            AddSectionErrorLabel.Text = "All fields must be filled out to add a section.";
            return;
        }

        List<string> sectionInfo = new List<string> {NameEntry.Text, CodeEntry.Text};
        string start = StartEntry.Text;
        string end = EndEntry.Text;
        List<DateOnly> dates = new List<DateOnly>{DateOnly.Parse(start), DateOnly.Parse(end)};


        // Connecting to the database through the view model to add the section, if the section could not be added displays reason
        string sectionValidation = await viewModel.AddSectionAsync(professorID, sectionInfo, dates);
        if(sectionValidation == "Success"){
            await DisplayAlert("New Section Added.", sectionInfo[0], "OK");
        }
        else{
            AddSectionErrorLabel.Text = sectionValidation;
            return;
        }
    }


    // If the professor chooses to edit a section, takes in the section and then calls a popup for editing 
    private async void OnEditSectionClicked(object sender, EventArgs e)
    {
        var section = (Section)((Button)sender).CommandParameter;
        var popup = new EditSectionPopup(viewModel, section);
        await this.ShowPopupAsync(popup);
    }


    // Allows the professor to delete a section 
    private async void OnDeleteSectionClicked(object sender, EventArgs e)
    {
        var section = (Section)((Button)sender).CommandParameter;
        string sectionCode = section.code;

        // Makes the professor confirm they wish to delete the section, and shows the affects it will have
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