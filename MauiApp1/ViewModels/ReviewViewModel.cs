/*
    ReviewModel Class
        Handles accessing all the teams their information for the peer reviews

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID: sny200000
*/

namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
public class ReviewViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;

    private List<Team> teams = new List<Team>();
    private List<Student> members = new List<Student>();
    private List<Score> scores = new List<Score>();
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Team> Teams{
        get => teams; 
        set{
            teams = value;
            OnPropertyChanged(nameof(Teams));
        }
    }
    public List<Student> Members{
        get => members;
        set{
            members = value;
            OnPropertyChanged(nameof(Members));
        }
    }
    public List<Score> Scores{
        get => scores;
        set{
            scores = value;
            OnPropertyChanged(nameof(Scores));
        }
    }
    public ReviewViewModel(string code)
    {
        databaseService = new DatabaseService();
        _ = InitializeAsync(code);
    }

    private async Task InitializeAsync(string code)
    {
        await LoadTeamsAsync(code);
    }
    //calls database service that retrieves teams and members based on a section and adds to 
    //team class for the professor
    private async Task LoadTeamsAsync(string code){
        var curr_teams = await databaseService.GetTeams(code);
        var new_teams = new List<Team>();
        var students = new List<Student>();

        foreach (var team in curr_teams)
        {
            team.members = new List<Student>();
            var membersForTeam = await databaseService.GetTeamMembers(code, team.number);
            foreach(var member in membersForTeam){
                var student = await databaseService.GetStudentsInfo(member.netid);
                team.members.Add(student);
            }
           new_teams.Add(team);
        }

        Teams = new_teams;
    }


    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}