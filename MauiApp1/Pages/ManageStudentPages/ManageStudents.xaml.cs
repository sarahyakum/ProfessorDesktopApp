/*
    Add Students .cs
        Allows the professor to add students to the database.
        When the page pulls up, it had the option to upload a CSV file, add an individual student, and displays the students currently in the class

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Project, Started on November 15th, 2024
        NetID: ech210001

*/

using MauiApp1.ViewModels;
using MauiApp1.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using MauiApp1.Pages.ManageStudents;
namespace MauiApp1.Pages.ManageStudentPages;

public partial class ManageStudents : ContentPage
{
    readonly string section;
    string name;
    string netid;
    string utdid;

    private readonly ManageStudentsViewModel viewModel;

    public ManageStudents(string sectionCode)
    {
        InitializeComponent();
        section = sectionCode;
        viewModel = new ManageStudentsViewModel(section);
        BindingContext = viewModel;
    }


    // When the professor tries to add a single student from the test fields 
    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        // Checking whether the name, netid, or utdid are empty before moving on
        if(string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(NetIDEntry.Text) || string.IsNullOrWhiteSpace(UTDIDEntry.Text))
        {
            AddStudentErrorLabel.Text = "Name, NetID, and UTDID must be filled out in order to add student.";
            return;
        }

        name = NameEntry.Text;
        netid = NetIDEntry.Text;
        utdid = UTDIDEntry.Text;
        string teamNumber = TeamEntry.Text;

        List<string> studentInfo = new List<string> {name, netid, utdid};


        // Calling the viewmodel to add the student to this section 
        string studentValidation = await viewModel.AddStudentAsync(section, studentInfo);
        
        if(studentValidation == "Success"){
            await DisplayAlert("New Student Added.", studentInfo[0], "OK");
        }
        else{
            AddStudentErrorLabel.Text = studentValidation;
            return;
        }


        // Adding the student to a team is optional, so testing whether the student has been assigned
        if(teamNumber != null)
        {
            List<string> teamInfo = new List<string> {netid, teamNumber};
            
            // Adding the student to a team as well, if the team does not exist then creates the team and then adds the student
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
    }

    // When professor wants to upload a CSV file with just the student information 
    private async void OnUploadCsvClicked(object sender, EventArgs e)
    {
        try
        {
            var fileResult = await FilePicker.PickAsync();

            // If the file uploaded correctly 
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

                    // Getting the information from the csv, to call the viewmodel to add the student 
                    if(columns.Length == 4)
                    {
                        string lastName = columns[0];
                        string firstName = columns[1];
                        string name = $"{firstName} {lastName}";
                        string netid = columns[2];
                        string utdid = columns[3];
                        
                        List<string> studentInfo = new List<string> {name, netid, utdid};
                        string studentValidation = await viewModel.AddStudentFromCSVAsync(section, studentInfo);

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
                    await viewModel.GetStudentsAsync(section);
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


    // When professor wants to upload a CSV file with the student information and the team assignment 
    private async void OnUploadStudentAndTeamClicked(object sender, EventArgs e)
    {
        try
        {
            var fileResult = await FilePicker.PickAsync();

            // If the file is uploaded correctly 
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
                        string studentValidation = await viewModel.AddStudentFromCSVAsync(section, studentInfo);

                        if(studentValidation != "Success")
                        {
                            failedStudents.Add($"{name}: {studentValidation}");
                        }

                        // Checking whether the team exists, if not creating it, then adding the student to the team
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
                    await viewModel.GetStudentsAsync(section);
                }

                // If there were any teams that were not able to be added to their team
                if(failedTeams.Any())
                {
                    string errorMessage = string.Join("\n", failedTeams);
                    await DisplayAlert("Students Not Added to Team", errorMessage, "OK");
                }
                else{
                    await DisplayAlert("Teams Assigned", "All students were added to a team!", "OK");
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


    // If the professor wants to edit an individual student 
    private async void OnEditStudentClicked(object sender, EventArgs e)
    {
        var student = (Student)((Button)sender).CommandParameter;
        var popup = new EditStudentPopup(viewModel, student);
        await this.ShowPopupAsync(popup);
    }


    // If the professor chooses to delete an individual student
    private async void OnDeleteStudentClicked(object sender, EventArgs e)
    {
        var student = (Student)((Button)sender).CommandParameter;
        string studentnetid = student.netid;

        // Confirming they wish to delete the student and all of their related data
        bool isConfirmed = await DisplayAlert("Delete Student", $"Are you sure you want to delete student {studentnetid}? They will be removed from the team and all of their timeslotes and Peer Reviews will be deleted.", "OK", "Cancel");
    
        if(isConfirmed)
        {
            string deleteValidation = await viewModel.DeleteStudentAsync(studentnetid);

            if(deleteValidation == "Success")
            {
                await DisplayAlert("Student Deleted", "Sudent deleted successfully", "OK");
            }
            else{
                await DisplayAlert("Student Not Deleted", "Student was unsuccessfully deleted", "OK");
            }
        }
    }
}