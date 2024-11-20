/*
    Edit Criteria Popup:
        Displays the current information about the criteria and allows the professor to edit it

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.PRSetUpPages;


public partial class EditCriteriaPopup : Popup
{
    string sectionCode;
    public Criteria criteriaPassed;
    public EditCriteriaPopup(Criteria criteria)
    {
        InitializeComponent();
        criteriaPassed = criteria;
        NameEntry.Text = criteria.name;
        DescriptionEntry.Text = criteria.description;
        TypeEntry.Text = criteria.reviewType;

        sectionCode = criteria.section;

    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        string updatedName = NameEntry.Text;
        string updatedDescription = DescriptionEntry.Text;
        string updatedType = TypeEntry.Text;

        Close(new Criteria
        {
            name = updatedName,
            description = updatedDescription, 
            reviewType = updatedType, 
            section = sectionCode
        });
    }

    // If they choose their mind about editing, returns the same criteria
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(criteriaPassed);
    }

}