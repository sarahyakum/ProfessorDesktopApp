/*
    Add Students .cs
        Allows the professor to add students to the database.
        When the page pulls up, it had the option to upload a CSV file, add an individual student, and displays the students currently in the class

    Written by Emma Hockett for CS 4485.0W1 Senior Design Project, Started on November 15th, 2024
        NetID: ech210001

*/

using MauiApp1.ViewModels;
using MauiApp1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace MauiApp1.Pages;

public partial class AssignTeams : ContentPage
{
    string section;
    string netid;
    string team;

    private AssignTeamsViewModel viewModel;

    public AssignTeams(string sectionCode)
    {
        InitializeComponent();
        section = sectionCode;
        viewModel = new AssignTeamsViewModel(section);
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
}