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

public partial class ManageStudents : ContentPage
{
    string section;
    string name;
    string netid;
    string utdid;

    private ManageStudentsViewModel viewModel;

    public ManageStudents(string sectionCode)
    {
        InitializeComponent();
        section = sectionCode;
        viewModel = new ManageStudentsViewModel(section);
        BindingContext = viewModel;
    }

    // When the professor tries to add a singular student through the input boxes
    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        name = NameEntry.Text;
        netid = NetIDEntry.Text;
        utdid = UTDIDEntry.Text;
        string teamNumber = TeamEntry.Text;

        List<string> studentInfo = new List<string> {name, netid, utdid};


        if(teamNumber != null)
        {
            List<string> teamInfo = new List<string> {netid, teamNumber};
            string studentValidation = await viewModel.AddStudentAsync(section, studentInfo);

            if(studentValidation == "Success"){
                await DisplayAlert("New Student Added.", studentInfo[0], "OK");
            }
            else{
                await DisplayAlert("Error adding section", studentValidation, "OK");
            }

            string teamValidation = await viewModel.CheckTeamExistsAsync(section, teamNumber);
            if(teamValidation == "Team doesn't exist")
            {
                teamValidation = await viewModel.CreateTeamAsync(section, teamNumber);
            }

            teamValidation = await viewModel.AssignTeamAsync(section, teamInfo);

            if(teamValidation != "Success")
            {
                await DisplayAlert("Couldn't add student to team", name, "OK");
            }            

        }
        else{
            string studentValidation = await viewModel.AddStudentAsync(section, studentInfo);
        
            if(studentValidation == "Success"){
                await DisplayAlert("New Student Added.", studentInfo[0], "OK");
            }
            else{
                await DisplayAlert("Error adding section", studentValidation, "OK");
            }
        }
    }

    // When professor wants to upload a CSV file with the student information 
    private async void OnUploadCsvClicked(object sender, EventArgs e)
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

                // Parsing each line in the csv and calling for the student to be added to the database
                foreach(var line in lines)
                {
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var columns = line.Split(',').Select(col => col.Trim()).Where(col => !string.IsNullOrWhiteSpace(col)).ToArray();

                    // Getting the information from the csv, to call the database function 
                    if(columns.Length == 4)
                    {
                        string lastName = columns[0];
                        string firstName = columns[1];
                        string name = $"{firstName} {lastName}";
                        string netid = columns[2];
                        string utdid = columns[3];
                        
                        List<string> studentInfo = new List<string> {name, netid, utdid};
                        string studentValidation = await viewModel.AddStudentAsync(section, studentInfo);

                        if(studentValidation != "Success")
                        {
                            failedStudents.Add($"{name}: {studentValidation}");
                        }
                    }
                }

                // If the student was unable to be added to the databse, displays their name and the reason
                if(failedStudents.Any())
                {
                    string errorMessage = string.Join("\n", failedStudents);
                    await DisplayAlert("Students Not Added", errorMessage, "OK");
                }
                else{
                    await DisplayAlert("All students added", "All students were added!", "OK");
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

       // When professor wants to upload a CSV file with the student information 
    private async void OnUploadStudentAndTeamClicked(object sender, EventArgs e)
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
                List<string> failedTeams = new List<string>();

                // Parsing each line in the csv and calling for the student to be added to the database
                foreach(var line in lines)
                {
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var columns = line.Split(',').Select(col => col.Trim()).Where(col => !string.IsNullOrWhiteSpace(col)).ToArray();

                    // Getting the information from the csv, to call the database function 
                    if(columns.Length == 5)
                    {
                        string lastName = columns[0];
                        string firstName = columns[1];
                        string name = $"{firstName} {lastName}";
                        string netid = columns[2];
                        string utdid = columns[3];
                        string teamNumber = columns[4];
                        
                        List<string> studentInfo = new List<string> {name, netid, utdid};
                        List<string> teamInfo = new List<string> {netid, teamNumber};
                        string studentValidation = await viewModel.AddStudentAsync(section, studentInfo);

                        if(studentValidation != "Success")
                        {
                            failedStudents.Add($"{name}: {studentValidation}");
                        }

                        string teamValidation = await viewModel.CheckTeamExistsAsync(section, teamNumber);
                        if(teamValidation == "Team doesn't exist")
                        {
                            teamValidation = await viewModel.CreateTeamAsync(section, teamNumber);
                        }

                        teamValidation = await viewModel.AssignTeamAsync(section, teamInfo);

                        if(teamValidation != "Success")
                        {
                            failedTeams.Add($"{netid}: {teamValidation}");
                        }

                    }
                }

                // If the student was unable to be added to the databse, displays their name and the reason
                if(failedStudents.Any())
                {
                    string errorMessage = string.Join("\n", failedStudents);
                    await DisplayAlert("Students Not Added", errorMessage, "OK");
                }
                else{
                    await DisplayAlert("All students added", "All students were added!", "OK");
                }

                
                if(failedTeams.Any())
                {
                    string errorMessage = string.Join("\n", failedTeams);
                    await DisplayAlert("Students Not Added to Team", errorMessage, "OK");
                }
                else{
                    await DisplayAlert("All students added", "All students were added!", "OK");
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