/*
    AddStudentsViewModel
        The view model for adding students to a section

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Project, Started on November 15, 2024
        NetID: ech210001

*/


namespace CS4485_Team75.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CS4485_Team75.Models;

public class ManageStudentsViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService databaseService;
    private ObservableCollection<Student>? students;

    public string section;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Student>? Students{
        get => students;
        set{
            students = value;
            OnPropertyChanged(nameof(Students));
        }
    }

    public ManageStudentsViewModel(string sectionCode)
    {
        databaseService = new DatabaseService();
        section = sectionCode;
        Students = new ObservableCollection<Student>();
    }

    public async Task InitializeAsync()
    {
        await GetStudentsAsync(section);
    }

    // Getting the students and their information that are in a section 
    public async Task GetStudentsAsync(string section)
    {
        var studentList = await databaseService.GetStudentAndInfo(section);
        Students = new ObservableCollection<Student>(studentList);
    }


    // Adding the students to the databse using the database service file 
    public async Task<string> AddStudentAsync(string section, List<string> studentInfo)
    {
        string studentResultMessage = await databaseService.AddStudents(studentInfo[1], studentInfo[2], studentInfo[0], section);
        await GetStudentsAsync(section);
        return studentResultMessage;
    }


    // Adding the students to the databse using the database service file, adds the student but doesnt reload the list until they have all been added
    public async Task<string> AddStudentFromCSVAsync(string section, List<string> studentInfo)
    {
        string studentResultMessage = await databaseService.AddStudents(studentInfo[1], studentInfo[2], studentInfo[0], section);
        return studentResultMessage;
    }


    // Editing the student information 
    public async Task<string> EditStudentAsync(string studentID, List<string> studentInfo)
    {
        string editResultMessage = await databaseService.EditStudent(studentID, studentInfo);
        await GetStudentsAsync(section);
        return editResultMessage;
    }


    // Deleting a student
    public async Task<string> DeleteStudentAsync(string netid)
    {
        string deleteResultMessage = await databaseService.DeleteStudent(netid);
        await GetStudentsAsync(section);
        return deleteResultMessage;
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