/*
    TimesheetsViewModel Class
        View model for getting each students current recorded time sheet

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on ....
        NETID:sny200000
*/
using System.Linq.Expressions;

namespace MauiApp1.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MauiApp1.Models;
using MauiApp1.Pages;

public class TimesheetsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private ObservableCollection<Student> students = new ObservableCollection<Student>();
    
    private DateOnly currentWeekStart;
    private int weekOffset = 0;
    //private ObservableCollection<Timeslot> timeslots = new ObservableCollection<Timeslot>();
    private List<DateOnly> window = new List<DateOnly>();
    //private List<Student> students = new List<Student>();
    public event PropertyChangedEventHandler PropertyChanged;
    
    public ICommand MoveToNextWeekCommand { get; }
    public ICommand MoveToPreviousWeekCommand { get; }
    public ObservableCollection<Student> Students{
        get => students;
        set{
            students=value;
            OnPropertyChanged(nameof(Students));
        }
        
        }
        public DateOnly CurrentWeekStart
    {
        get => currentWeekStart;
         set
        {
            if (currentWeekStart != value)
            {
                currentWeekStart = value;
                OnPropertyChanged(nameof(CurrentWeekStart));
                UpdateCurrentWeekRange();

            }
        }
    }

    private string currentWeekRange;
    public string CurrentWeekRange
    {
        get => currentWeekRange;
        set
        {
            if (currentWeekRange != value)
            {
                currentWeekRange = value;
                OnPropertyChanged(nameof(CurrentWeekRange));
            }
        }
    }
    /*public ObservableCollection<Timeslot> Timeslots{
        get => timeslots;
        set{
            timeslots=value;
            OnPropertyChanged(nameof(Timeslots));
        }
        
        }*/
    public List<DateOnly> Window{
        get => window;
        set{
            window=value;
            OnPropertyChanged(nameof(Window));
        }
    }
    

    public TimesheetsViewModel(string section)
    {
        databaseService = new DatabaseService();
        MoveToNextWeekCommand = new Command(async () =>
        {
            await MoveToNextWeekAsync();
        });
        MoveToPreviousWeekCommand = new Command(async () =>
        {
            await MoveToPreviousWeekAsync();
        });

        _ = StartAsync(section);
        _ = GetTimeFrameAsync(section);
        
        
        
        

    }

    public async Task StartAsync(string section){
        CurrentWeekStart = GetStartOfWeek(DateOnly.FromDateTime(DateTime.Today));
        await LoadStudentsAsync(section);
    }
    private DateOnly GetStartOfWeek(DateOnly date)
    {
        int daysToSubtract = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
        return date.AddDays(-daysToSubtract);
    }


    public async Task LoadStudentsAsync(string code){
        
        Students = await databaseService.GetStudents(code);
        await LoadWeeklyTimeslotsAsync();


    }
    private void UpdateCurrentWeekRange()
    {
        var currentWeekEnd = currentWeekStart.AddDays(6);
        CurrentWeekRange = $"{currentWeekStart:MMM dd} - {currentWeekEnd:MMM dd}";
    }
    public async Task LoadWeeklyTimeslotsAsync()
    {
        try{
        var weekStart = CurrentWeekStart.AddDays(weekOffset * 7);
        var weekEnd = weekStart.AddDays(6);
        // Create a list of tasks for loading timeslots for each student
    var tasks = students.Select(async student =>
    {
        try{
        // Fetch timeslots for the week from the database for this student
        var timeslots = await databaseService.GetWeekTimeslots(weekStart, student.netid);
        // Check if student.timeslots is null before using it
            if (student.timeslots == null)
            {
                student.timeslots = new Dictionary<DateOnly, Timeslot>(); // Initialize if null
            }

        // Add or update the student's timeslots
        foreach (var timeslot in timeslots)
        {
            if(timeslot!=null){
                student.timeslots[timeslot.date] = timeslot;
            }
            
        }

        // Ensure that all dates in the week have a timeslot entry
        for (var date = weekStart; date <= weekEnd; date = date.AddDays(1))
        {
            if (!student.timeslots.ContainsKey(date))
            {
                student.timeslots[date] = new Timeslot
                {
                    date = date,
                    hours = "00:00",  // Default value
                    description = "No entry"
                };
            }
        }}
        catch(Exception ex){
            Console.WriteLine($"Error for student {student.netid}: {ex.Message}");
                // Log to a file or a crash reporting service
        }
    }).ToList();

    // Wait for all tasks to complete
    await Task.WhenAll(tasks);}
    catch(Exception ex){
        Console.WriteLine($"Error loading weekly timeslots: {ex.Message}");
        }}
    public async Task MoveToNextWeekAsync()
    {
        weekOffset++;
        CurrentWeekStart = CurrentWeekStart.AddDays(7);
        await LoadWeeklyTimeslotsAsync();
    }
    public async Task MoveToPreviousWeekAsync()
    {
        weekOffset--;
        CurrentWeekStart = CurrentWeekStart.AddDays(-7);
        await LoadWeeklyTimeslotsAsync();
    }

    //Retrieves courses time frame
    public async Task GetTimeFrameAsync(string section){
        List<DateOnly> curr_window = await databaseService.GetCourseTimeFrame(section);
        Window = curr_window;
        await LoadStudentsAsync(section);
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
