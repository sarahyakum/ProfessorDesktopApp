/*
    Edit STeam Popup:
        Allows the professor to edit the team number 

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 20, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.ManageTeams;


public partial class EditTeamPopup : Popup
{

    public Team teamPassed;
    public EditTeamPopup(Team team)
    {
        InitializeComponent();
        teamPassed = team;
        NumberEntry.Text = (team.number).ToString();
    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        string updatedNumber = NumberEntry.Text;

        Close(new Team
        {
            number = int.Parse(updatedNumber),
            section = teamPassed.section,
            members = teamPassed.members
        });
    }

    // If they choose their mind about editing, returns the same student
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(teamPassed);
    }

}