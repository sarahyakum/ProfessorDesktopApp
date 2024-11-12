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
        LoadTeamsAsync(code);
    }
    //calls database service that retrieves teams and members based on a section and adds to 
    //team class for the professor
    private async void LoadTeamsAsync(string code){
        Teams = await databaseService.GetTeams(code);
        List<Team> new_teams = new List<Team>();

        foreach (Team team in teams){
            LoadMembersAsync(code, team.number);
            team.members = Members;
            new_teams.Add(team);


        }
        Teams = new_teams;
    }
    //Calls database service to add students to their respective teams
    private async void LoadMembersAsync(string code, int number){
        Members = await databaseService.GetTeamMembers(code, number);
        List<Student> new_members = [.. members];
        Members = new_members;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}