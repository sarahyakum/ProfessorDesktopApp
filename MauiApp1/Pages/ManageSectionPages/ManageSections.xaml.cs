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
    string sectionName;
    string sectionCode;
    DateTime startdate;
    DateTime enddate;

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
        List<string> sectionInfo = new List<string> {NameEntry.Text, CodeEntry.Text};
        string start = StartEntry.Text;
        string end = EndEntry.Text;

        List<DateOnly> dates = new List<DateOnly>{DateOnly.Parse(start), DateOnly.Parse(end)};

        string sectionValidation = await viewModel.AddSectionAsync(professorID, sectionInfo, dates);
        
        if(sectionValidation == "Success"){
            await DisplayAlert("New Section Added.", sectionInfo[0], "OK");
        }
        else{
            await DisplayAlert("Error adding section", sectionValidation, "OK");
        }
    }

    // For editing the section 
    private async void OnEditSectionClicked(object sender, EventArgs e)
    {
        var section = (Section)((Button)sender).CommandParameter;
        string sectionCode = section.code;
        var popup = new EditSectionPopup(section);
        var result = await this.ShowPopupAsync(popup) as Section;


        List<string> updatedInfo = new List<string>{result.name, result.code};
        List<DateOnly> updatedDates = new List<DateOnly>{result.startDate, result.endDate};

        string editValidation = await viewModel.EditSectionAsync(sectionCode, updatedInfo, updatedDates);
        

        if(result != null)
        {
            Console.WriteLine("Success");
        }
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
                await DisplayAlert("Section Not Deleted", deleteValidation, "OK");
            }
        }
        else{
            return;
        }
    }

}