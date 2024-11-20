/*
    AddStudentsViewModel
        The view model for adding students to a section

    Written by Emma Hockett for CS 4485.0W1 Senior Design Project, Started on November 15, 2024
        NetID: ech210001

*/


namespace MauiApp1.ViewModels;
using System.ComponentModel;
using MauiApp1.Models;


public class ManageStudentsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private List<Student> students;
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Student> Students{
        get => students;
        set{
            students = value;
            OnPropertyChanged(nameof(Students));
        }
    }

    public ManageStudentsViewModel(string section)
    {
        databaseService = new DatabaseService();
        GetStudentsAsync(section);
    }

    // Getting the students that are in a section 
    public async void GetStudentsAsync(string section)
    {
        Students = await databaseService.GetStudents(section);
        GetStudentsInfoAsync(Students);
    }

    // Getting the information about the students in the section 
    public async void GetStudentsInfoAsync(List<Student> students)
    {
        foreach (var student in students)
        {
            var detailedInfo = await databaseService.GetStudentsInfo(student.netid);
            student.name = detailedInfo.name;
            student.netid = detailedInfo.netid;
            student.utdid = detailedInfo.utdid;
        }
        OnPropertyChanged(nameof(Students));
    }

    // Adding the students to the databse using the database service file 
    public async Task<string> AddStudentAsync(string section, List<string> studentInfo)
    {
        string studentResultMessage = await databaseService.AddStudents(studentInfo[1], studentInfo[2], studentInfo[0], section);
        return studentResultMessage;
    }

    // Checking if the team number already exists for this section
    public async Task<string> CheckTeamExistsAsync(string section, string teamNum)
    {
        int teamNumber = int.Parse(teamNum);
        string teamResultMessage = await databaseService.CheckTeamExists(section, teamNumber);
        return teamResultMessage;
    }

    // Adding the team number to the database if it needs to be added
    public async Task<string> CreateTeamAsync(string section, string teamNum)
    {
        int teamNumber = int.Parse(teamNum);
        string teamResultMessage = await databaseService.InsertTeamNum(section, teamNumber);
        return teamResultMessage;
    }

    // Assigning the students to a team with the database service file 
    public async Task<string> AssignTeamAsync(string section, List<string> teamInfo)
    {
        int teamNumber = int.Parse(teamInfo[1]);
        string teamResultMessage = await databaseService.AddNewTeamMember(teamNumber, teamInfo[0],section);
        return teamResultMessage;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}