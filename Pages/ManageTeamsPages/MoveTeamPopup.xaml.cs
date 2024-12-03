/*
    Move Team Popup:
        Allows the student to be moved to a new team

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
namespace CS4485_Team75.Pages.ManageTeams;


public partial class MoveTeamPopup : Popup
{
    readonly string studentID;
    readonly int teamNumber;
    readonly string section;
    private readonly ManageTeamsViewModel viewModel;
    public MoveTeamPopup(ManageTeamsViewModel viewModel, string netid, int teamNum, string sectionCode)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        studentID = netid;
        teamNumber = teamNum;
        section = sectionCode;
        TeamEntry.Text = teamNum.ToString();
    }


    // If the changes want to be saved 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {
        int teamNum = int.Parse(TeamEntry.Text);

        // Checking whether the new team field has been filled out 
        if(string.IsNullOrWhiteSpace(TeamEntry.Text))
        {
            ErrorLabel.Text = "All fields must be filled out to move the student.";
            return;
        }
        else if(teamNumber == teamNum) 
        {
            // If no changes were made 
            Close();
        }
        
        // Either changing the team or presenting the condition not met
        string changeValidation = await viewModel.ChangeTeamAsync(section, studentID, teamNum);
        if(changeValidation == "Success")
        {
            Close();
        }
        else{
            ErrorLabel.Text = changeValidation;
            return;
        }
    }

    // If they choose their mind about editing, closes the popup
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

}