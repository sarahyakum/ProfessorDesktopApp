/*
    Reuse Criteria Popup:
        Allows the professor to select a criteria from a previous review type and add it with a new type

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.PRSetUp;


public partial class ReuseCriteriaPopup : Popup
{
    string sectionCode;
    public Criteria criteriaPassed;
    public ReuseCriteriaPopup(Criteria criteria)
    {
        InitializeComponent();
        criteriaPassed = criteria;
        TypeEntry.Text = criteria.reviewType;

        sectionCode = criteria.section;
    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        string updatedType = TypeEntry.Text;

        Close(new Criteria
        {
            name = criteriaPassed.name,
            description = criteriaPassed.description,
            reviewType = updatedType,
            section = criteriaPassed.section
        });
    }

    // If they choose their mind about editing, returns the same section 
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(criteriaPassed);
    }

}