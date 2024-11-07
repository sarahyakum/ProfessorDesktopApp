namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Pages;

public class TimeslotViewModel : INotifyPropertyChanged
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

    public TimeslotViewModel(string netid)
    {
        DateTime date = new DateTime(2024, 10, 8);;
        databaseService = new DatabaseService();
        GetTimeslotsAsync(date, netid);

    }

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
