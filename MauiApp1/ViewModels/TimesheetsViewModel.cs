/*
    TimesheetsViewModel Class
        View model for getting each students current recorded time sheet

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID:sny200000
*/
namespace MauiApp1.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Pages;

public class TimesheetsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private ObservableCollection<Timeslot> timeslots = new ObservableCollection<Timeslot>();
    private List<DateTime> window = new List<DateTime>();
    private List<Student> students = new List<Student>();
    public event PropertyChangedEventHandler PropertyChanged;
    public List<Student> Students{
        get => students;
        set{
            students=value;
            OnPropertyChanged(nameof(Students));
        }
        
        }
    public ObservableCollection<Timeslot> Timeslots{
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
    

    public TimesheetsViewModel(string section)
    {
        databaseService = new DatabaseService();
        LoadStudentsAsync(section);
        GetTimeFrameAsync(section);
        //DateTime startDate = Window[0];
        //DateTime endDate = Window[1];
        
        //GetTimeslotsAsync(netid, startDate, endDate);
        

    }
     //Add students based on their section for their timesheets
    private async void LoadStudentsAsync(string code)
    {
        Students = await databaseService.GetStudents(code);
        List<Student> new_students=new List<Student>();
        foreach (Student student in Students){
            string netid = student.netid;
            
            Student stu = await databaseService.GetStudentsInfo(netid);
            new_students.Add(stu);
            string name = stu.name;
            GetTimeslotsAsync(netid, name, Window[0], Window[1]);

        }
        Students = new_students;
        
        
    }

    //Retrieves each students current recorded timesheets
    private async void GetTimeslotsAsync(string netid, string name, DateTime startDate, DateTime endDate)
    {
        DateTime currDate = startDate;
        for(DateTime i = startDate; i <= endDate; i.AddDays(1)){
            Timeslots = await databaseService.GetTimeslots(i,netid);
            //var student = students.FirstOrDefault(s => s.netid == netid);
            //Timeslot timeslot = new Timeslot();
            Student curr_student =  new Student();
            curr_student.netid = netid;
            curr_student.name = name;
            foreach(Timeslot slot in Timeslots){
                curr_student.timeslots.Add(slot);
            }

            Timeslots = curr_student.timeslots;

        }
        
        
        
    }

    //Retrieves courses time frame
    private async void GetTimeFrameAsync(string section){
        List<DateTime> curr_window = await databaseService.GetCourseTimeFrame(section);
        Window = curr_window;
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
