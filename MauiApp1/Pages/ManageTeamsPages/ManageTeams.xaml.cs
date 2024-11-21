/*
    Manage Teams Page
        Allows the professor to upload a CSV file containing the team assignments for sections
        Allows the professor to assign a singular student to a team 
        Shows a list of the current teams and their students 
        Allows the professor to Move a student to a different team
        Allows the professor to Remove a student from a team without adding them to a different team 
        Allows the professor to change a team number 
        Allows the professor to delete a team 

    Written by Emma Hockett for CS 4485.0W1 Senior Design Project, Started on November 15th, 2024
        NetID: ech210001

*/

using MauiApp1.ViewModels;
using MauiApp1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using MauiApp1.Pages.ManageTeams;
using CommunityToolkit.Maui.Views;
//using Microsoft.UI.Xaml.Controls;
namespace MauiApp1.Pages.ManageTeamsPages;

public partial class ManageTeams : ContentPage
{
    string section;
    string netid;
    string team;

    private ManageTeamsViewModel viewModel;

    public ManageTeams(string sectionCode)
    {
        InitializeComponent();
        section = sectionCode;
        viewModel = new ManageTeamsViewModel(section);
        BindingContext = viewModel;
    }

    // When the professor assigns a singular student through the input fields 
    private async void OnAssignStudentClicked(object sender, EventArgs e)
    {
        // Checking whether the netid and team number fields are filled out
        if(string.IsNullOrWhiteSpace(NetIDEntry.Text) || string.IsNullOrWhiteSpace(TeamNumEntry.Text))
        {
            AssignTeamErrorLabel.Text = "All fields must be filled to assign student";
            return;
        }

        netid = NetIDEntry.Text;
        team = TeamNumEntry.Text;
        List<string> teamInfo = new List<string> {netid, team};

        // Checks whether the team exists already or a new one needs to be made
        string teamValidation = await viewModel.CheckTeamExistsAsync(section, teamInfo[1]);
        if(teamValidation == "Team doesn't exist")
        {
            teamValidation = await viewModel.CreateTeamAsync(section, teamInfo[1]);
        }
        
        teamValidation = await viewModel.AssignTeamAsync(section, teamInfo);
        
        if(teamValidation == "Success"){
            await DisplayAlert("Student Assigned to Team", teamInfo[0], "OK");
        }
        else{
            AssignTeamErrorLabel.Text = teamValidation;
            return;
        }
    }

    // When professor wants to upload a CSV file with the student team assignments 
    private async void OnUploadCSVTeamsClicked(object sender, EventArgs e)
    {
        try
        {
            var fileResult = await FilePicker.PickAsync();

            if(fileResult != null)
            {
                var filePath = fileResult.FullPath;

                // Checking whether they uploaded a CSV file 
                if(!filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    await DisplayAlert("Invalid File Type", "Please select a CSV file.", "OK");
                    return;
                }

                var fileContent = await File.ReadAllTextAsync(filePath);

                var lines = fileContent.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line) && line.Trim(',').Length > 0).ToList();
                lines = lines.Skip(1).ToList();
                
                List<string> failedStudents = new List<string>();

                // Parsing each line in the csv 
                foreach(var line in lines)
                {
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var columns = line.Split(',').Select(col => col.Trim()).Where(col => !string.IsNullOrWhiteSpace(col)).ToArray();

                    // Getting the information from the csv, to call the database function 
                    if(columns.Length == 2)
                    {
                        string netid = columns[0];
                        string team = columns[1];
                        
                        List<string> teamInfo = new List<string> {netid, team};

                        // Checking whether the team exists yet and if not adding it
                        string teamValidation = await viewModel.CheckTeamExistsAsync(section, teamInfo[1]);
                        if(teamValidation == "Team doesn't exist")
                        {
                            teamValidation = await viewModel.CreateTeamAsync(section, teamInfo[1]);
                        }
                        teamValidation = await viewModel.AssignTeamAsync(section, teamInfo);

                        if(teamValidation != "Success")
                        {
                            failedStudents.Add($"{netid}: {teamValidation}");
                        }
                    }
                }

                // If the student was unable to be assigned to team, then display their netid and the reason
                if(failedStudents.Any())
                {
                    string errorMessage = string.Join("\n", failedStudents);
                    await DisplayAlert("Students Not Assigned", errorMessage, "OK");
                }
                else{
                    await DisplayAlert("All students assigned", "All students were assigned!", "OK");
                }
            }
            else
            {
                await DisplayAlert("No File Selected", "Please select a valid CSV file.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    // Allows the professor to move the student to a different team 
    private async void OnMoveTeamsClicked(object sender, EventArgs e)
    {
        string netid = (string)((Button)sender).CommandParameter;
        int teamNum = await viewModel.GetTeamNumberAsync(netid);

        string checkPR = await viewModel.CheckPeerReviewStatusAsync(teamNum, section);

        if(checkPR == "Peer Reviews have already been created")
        {
            await DisplayAlert("Cannot Move Team", "A peer review has already been created for this team, so the members cannot be changed.", "OK");
            return;
        }

        var popup = new MoveTeamPopup(viewModel, netid, teamNum, section);
        var result = (List<string>)await this.ShowPopupAsync(popup);
    }

    // Removing a student from the team without moving them to a new one
    private async void OnRemoveFromTeamClicked(object sender, EventArgs e)
    {
        string netid = (string)((Button)sender).CommandParameter;
        int teamNum = await viewModel.GetTeamNumberAsync(netid);

        // Checks whether a peer review has already been created, if so the students cannot be removed from the team
        string checkPR = await viewModel.CheckPeerReviewStatusAsync(teamNum, section);

        if(checkPR == "Peer Reviews have already been created")
        {
            await DisplayAlert("Cannot Move Team", "A peer review has already been created for this team, so the members cannot be changed.", "OK");
            return;
        }

        // Confirming whether they want to remove the student
        bool isConfirmed = await DisplayAlert("Remove Student From Team", $"Are you sure you want to remove {netid}?", "OK", "Cancel");
        if(isConfirmed)
        {
            string deleteValidation = await viewModel.RemoveFromTeamAsync(netid, section);
            
            if(deleteValidation == "Success")
            {
                await DisplayAlert("Student Removed", "Sudent removed successfully", "OK");
            }
            else{
                await DisplayAlert("Student Not Removed", deleteValidation, "OK");
            }
        }
    }

    // Allows the professor to change the team number 
    private async void OnEditTeamClicked(object sender, EventArgs e)
    {
        var team = (Team)((Button)sender).CommandParameter;
        int teamNumber = team.number;

        var popup = new EditTeamPopup(viewModel, team, section);
        var result = await this.ShowPopupAsync(popup) as Team;

    }

    // Allows the professor to delete a team only if a peer review with this team has not already been created 
    private async void OnDeleteTeamClicked(object sender, EventArgs e)
    {
        var team = (Team)((Button)sender).CommandParameter;

        // Checks whether the team has been used in a peer review already, if so it cannot be deleted
        string checkPR = await viewModel.CheckPeerReviewStatusAsync(team.number, section);
        if(checkPR == "Peer Reviews have already been created")
        {
            await DisplayAlert("Cannot Delete Team", "A peer review has already been created for this team, so it cannot be deleted.", "OK");
            return;
        }

        bool isConfirmed = await DisplayAlert("Delete Team", $"Are you sure you want to delete team {team.number}? All members of the team will be removed.", "OK", "Cancel");
        if(isConfirmed)
        {
            string deleteValidation = await viewModel.DeleteTeamAsync(section, team.number);

            if(deleteValidation == "Success")
            {
                await DisplayAlert("Team Deleted", $"Team {team.number} was deleted", "OK");
            }
            else{
                await DisplayAlert("Team Not Deleted", $"Team {team.number} was not deleted", "OK");
            }
        }
    }
}