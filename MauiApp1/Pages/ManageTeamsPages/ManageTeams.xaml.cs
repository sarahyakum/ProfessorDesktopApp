/*
    Manage Teams Page
        Allows the professor to upload the team assignments through a csv file upload
            or enter them manually

        Displays a list of the current team assignments 

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
            await DisplayAlert("Error adding section", teamValidation, "OK");
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
        var popup = new MoveTeamPopup(netid, teamNum);

        var result = (List<string>)await this.ShowPopupAsync(popup);

        int updatedTeamNum = int.Parse(result[1]);

        string changeValidation = await viewModel.ChangeTeamAsync(section, netid, updatedTeamNum);

        if(changeValidation == "Success")
        {
            await DisplayAlert("Student Team Changed", $"{netid} was moved to team {updatedTeamNum}", "OK");
        }
        else{
            await DisplayAlert("Student Not Moved", changeValidation, "OK");
        }
    }

    private async void OnRemoveFromTeamClicked(object sender, EventArgs e)
    {
        string netid = (string)((Button)sender).CommandParameter;
        bool isConfirmed = await DisplayAlert("Remove Student From Team", $"Are you sure you want to remove {netid}? If a Peer Review has already been created they cannot be removed.", "OK", "Cancel");

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
}