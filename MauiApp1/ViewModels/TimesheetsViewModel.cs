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
    private List<DateTime> window = new List<DateTime>();
    //public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public List<Timeslot> Timeslots{
        get => timeslots;
        set{
            timeslots=value;
            OnPropertyChanged(nameof(Timeslots));
        }
        
    }
    public List<DateTime> Window{
        get => window;
        set{
            window=value;
            OnPropertyChanged(nameof(Window));
        }
    }

    public TimesheetsViewModel(string netid)
    {
        DateTime startDate = Window[0];
         DateTime endDate = Window[1];
        databaseService = new DatabaseService();
        GetTimeslotsAsync(netid, startDate, endDate);

    }
    //Retrieves each students current recorded timesheets
    private async void GetTimeslotsAsync(string netid, DateTime startDate, DateTime endDate)
    {
        DateTime currDate = startDate;
        for(DateTime i = startDate; i <= endDate; i.AddDays(1)){
            Timeslots = await databaseService.GetTimeslots(i,netid);
            Student curr_student =  new Student(){netid = netid};

            foreach(Timeslot slot in Timeslots){
                curr_student.timeslots.Add(slot);
            }

            Timeslots = curr_student.timeslots;

        }
    }
    //Retrieves courses time frame
    //private async void GetTimeFrameAsync()

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
