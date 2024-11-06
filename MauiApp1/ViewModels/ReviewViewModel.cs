/*
    ReviewModel Class
        NOT SURE WHAT TO ADD HERE 

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID:
*/

namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
public class ReviewViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;

    private List<Team> teams = new List<Team>();
    public event PropertyChangedEventHandler PropertyChanged;

    public List<Team> Teams{
        get => teams;
        set{
            teams = value;
            OnPropertyChanged(nameof(Teams));
        }
    }
    public ReviewViewModel(string code)
    {
        databaseService = new DatabaseService();
        LoadTeamsAsync(code);
    }
    private async void LoadTeamsAsync(string code){
        Teams = await databaseService.getTeams(code);
        List<Team> new_teams = new List<Team>();

        foreach (Team team in teams){
            int num = team.number;
            new_teams.Add(team);

        }
        Teams = new_teams;
    }
    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
