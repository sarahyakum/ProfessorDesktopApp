/*
    Move Team Popup:
        Allows the student to be moved to a new team

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.ManageTeams;


public partial class MoveTeamPopup : Popup
{
    string studentID;
    int teamNumber;
    public MoveTeamPopup(string netid, int teamNum)
    {
        InitializeComponent();
        studentID = netid;
        teamNumber = teamNum;
        TeamEntry.Text = teamNum.ToString();
    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        List<string>  updatedInfo = new List<string>{studentID, TeamEntry.Text};
        Close(updatedInfo);
    }

    // If they choose their mind about editing, returns the same information 
    private void OnCancelClicked(object sender, EventArgs e)
    {
        List<string> sameInfo = new List<string>{studentID, teamNumber.ToString()};
        Close(sameInfo);
    }

}