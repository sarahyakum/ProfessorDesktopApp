/*
    Edit Team Popup:
        Allows the professor to edit the team number and handles the logic of changing it 

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
namespace CS4485_Team75.Pages.ManageTeams;


public partial class EditTeamPopup : Popup
{

    public Team teamPassed;
    readonly string section;
    private readonly ManageTeamsViewModel viewModel;
    public EditTeamPopup(ManageTeamsViewModel viewModel, Team team, string sectionCode)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        teamPassed = team;
        section = sectionCode;
        NumberEntry.Text = team.number.ToString();
    }

    // If the changes want to be saved 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {
        // Checks whether the team number field is filled out
        if(string.IsNullOrWhiteSpace(NumberEntry.Text))
        {
            ErrorLabel.Text = "All fields must be filled out to edit team number";
            return;
        }
        else if(teamPassed.number == int.Parse(NumberEntry.Text))
        {
            // If no changes were made
            Close();
        }

        
        // Makes the edit or diaplays why the change couldn't be made
        string editValidation = await viewModel.EditTeamNumberAsync(section, teamPassed.number, int.Parse(NumberEntry.Text));
        
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