/*
    Edit Section Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.ManageSections;


public partial class EditSectionPopup : Popup
{
    string sectionCode;
    public Section sectionPassed;
    public EditSectionPopup(Section section)
    {
        InitializeComponent();
        sectionPassed = section;
        NameEntry.Text = section.name;
        CodeEntry.Text = section.code;
        StartDateEntry.Text = section.startDate.ToString("MM/dd/yyyy");
        EndDateEntry.Text = section.endDate.ToString("MM/dd/yyyy");

        sectionCode = section.code;
    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        string updatedName = NameEntry.Text;
        string updatedCode = CodeEntry.Text;
        DateOnly updatedStart = DateOnly.Parse(StartDateEntry.Text);
        DateOnly updatedEnd = DateOnly.Parse(EndDateEntry.Text);

        Close(new Section
        {
            name = updatedName,
            code = updatedCode, 
            startDate = updatedStart, 
            endDate = updatedEnd
        });
    }

    // If they choose their mind about editing, returns the same section 
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(sectionPassed);
    }

}