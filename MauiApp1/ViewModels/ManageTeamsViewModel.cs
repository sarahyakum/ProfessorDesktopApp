/*
    AssignTeamsViewModel
        The view model for assigning students to a team

    Written by Emma Hockett for CS 4485.0W1 Senior Design Project, Started on November 15, 2024
        NetID: ech210001

*/


namespace MauiApp1.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MauiApp1.Models;


public class ManageTeamsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private ObservableCollection<Team> teams;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Team> Teams{
        get => teams;
        set{
            teams = value;
            OnPropertyChanged(nameof(Teams));
        }
    }

    public ManageTeamsViewModel(string section)
    {
        databaseService = new DatabaseService();
        Teams = new ObservableCollection<Team>();
        LoadTeamsAsync(section);
    }

    
    // Getting the students on the various teams in the section 
    public async void LoadTeamsAsync(string section)
    {
        var allteams = await databaseService.GetTeams(section);
        var teams = allteams.Where(t => t.section == section).ToList();

        foreach(var team in teams)
        {
            var students = await databaseService.GetTeamMembers(section, team.number);
            foreach (var student in students)
            {
                var detailedInfo = await databaseService.GetStudentsInfo(student.netid);
                student.name = detailedInfo.name;
                student.netid = detailedInfo.netid;
                student.utdid = detailedInfo.utdid;
            }
            team.members = new List<Student>(students);
            Teams.Add(team);
        }
        OnPropertyChanged(nameof(Teams));
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
        string teamResultMessage = await databaseService.AddNewTeamMember(teamNumber, teamInfo[0], section);
        return teamResultMessage;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}