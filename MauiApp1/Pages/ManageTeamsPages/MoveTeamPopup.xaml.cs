/*
    Move Team Popup:
        Allows the student to be moved to a new team

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;
namespace MauiApp1.Pages.ManageTeams;


public partial class MoveTeamPopup : Popup
{
    string studentID;
    int teamNumber;
    string section;
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
        int teamNum = await viewModel.GetTeamNumberAsync(studentID);

        // Checking whether the new team field has been filled out 
        if(string.IsNullOrWhiteSpace(TeamEntry.Text))
        {
            ErrorLabel.Text = "All fields must be filled out to move the student.";
            return;
        }
        else if(teamNum == int.Parse(TeamEntry.Text))   // if nothing was changed 
        {
            List<string> sameInfo = new List<string>{studentID, teamNum.ToString()};
            Close(sameInfo);
        }
        
        // Either changing the team or presenting the condition not met
        string changeValidation = await viewModel.ChangeTeamAsync(section, studentID, int.Parse(TeamEntry.Text));
        if(changeValidation == "Success")
        {
            List<string>  updatedInfo = new List<string>{studentID, TeamEntry.Text};
            Close(updatedInfo);
        }
        else{
            ErrorLabel.Text = changeValidation;
            return;
        }

    }

    // If they choose their mind about editing, returns the same information 
    private void OnCancelClicked(object sender, EventArgs e)
    {
        List<string> sameInfo = new List<string>{studentID, teamNumber.ToString()};
        Close(sameInfo);
    }

}