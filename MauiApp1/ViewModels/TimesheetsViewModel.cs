/*
    TimesheetsViewModel Class
        View model for getting each students current recorded time sheet

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID:sny200000
*/
namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Pages;

public class TimesheetsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private List<Timeslot> timeslots = new List<Timeslot>();
    public event PropertyChangedEventHandler PropertyChanged;
    
    public List<Timeslot> Timeslots{
        get => timeslots;
        set{
            timeslots=value;
            OnPropertyChanged(nameof(Timeslots));
        }
        
        }

    public TimesheetsViewModel(string netid)
    {
        DateTime date = new DateTime(2024, 10, 8);;
        databaseService = new DatabaseService();
        GetTimeslotsAsync(date, netid);

    }
    //Retrieves each students current recorded timesheets
    private async void GetTimeslotsAsync(DateTime date, string netid)
    {
        Timeslots = await databaseService.GetTimeslots(date,netid);
        Student curr_student =  new Student();
        curr_student.netid = netid;
        foreach(Timeslot slot in Timeslots){
            curr_student.timeslots.Add(slot);
        }

        Timeslots = curr_student.timeslots;
        
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
