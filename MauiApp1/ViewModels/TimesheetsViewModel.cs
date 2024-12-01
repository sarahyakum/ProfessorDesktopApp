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
    
    //Handles week changes
    public event EventHandler? WeekChanged;
    public ICommand MoveToNextWeekCommand { get; }
    public ICommand MoveToPreviousWeekCommand { get; }
    
    
    private DateOnly currentWeekStart;
    public string? currentWeekRange;
    private int weekOffset = 0;
    private List<DateOnly> window = new List<DateOnly>();
    private ObservableCollection<Student> students = new ObservableCollection<Student>();
    public event PropertyChangedEventHandler? PropertyChanged;
    
    //Sets list of students
    public ObservableCollection<Student> Students{
        get => students;
        set{
            students=value;
            OnPropertyChanged(nameof(Students));
        }
        
    }

    //Sets up sections course time frame
    public List<DateOnly> Window{
        get => window;
        set{
            window=value;
            OnPropertyChanged(nameof(Window));
        }
    }

    //Sets the current week
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

    //Sets up the range header
    public string CurrentWeekRange
    {
        get => currentWeekRange?? "";
        set
        {
            if (currentWeekRange != value)
            {
                currentWeekRange = value;
                OnPropertyChanged(nameof(CurrentWeekRange));
            }
        }
    }
    

    
    //Handles acquire data for the timesheets page
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

    //Begin the functionality
    public async Task StartAsync(string section){
        if (currentWeekStart == DateOnly.MinValue)
        {
            CurrentWeekStart = GetStartOfWeek(DateOnly.FromDateTime(DateTime.Today));
        }

        // Load students and initialize data
        
        await LoadStudentsAsync(section);
        
    }
    //get Monday of the week
    private DateOnly GetStartOfWeek(DateOnly date)
    {
        int daysToSubtract = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
        return date.AddDays(-daysToSubtract);
    }

    //Gets student information from database
    public async Task LoadStudentsAsync(string code){
        
        Students = await databaseService.GetStudentAndInfo(code);
        await LoadWeeklyTimeslotsAsync();


    }

    //changes the range
    private void UpdateCurrentWeekRange()
    {
        var currentWeekEnd = currentWeekStart.AddDays(6);
        CurrentWeekRange = $"{currentWeekStart:MMM dd} - {currentWeekEnd:MMM dd}";
    }

    //Accesses the database to get the current weeks timesheets
    public async Task LoadWeeklyTimeslotsAsync()
    {
        try{
        var weekStart = CurrentWeekStart;
        var weekEnd = weekStart.AddDays(6);

        //Acquire timeslots for all students
        var tasks = students.Select(async student =>
        {
            try{
            var timeslots = await databaseService.GetWeekTimeslots(weekStart, student.netid);
            
            if (student.timeslots == null)
            {
                student.timeslots = new Dictionary<DateOnly, Timeslot>(); // Initialize if null
            }

            //update time slots
            foreach (var timeslot in timeslots)
            {
                if(timeslot!=null){
                    student.timeslots[timeslot.date] = timeslot;
                }
                
            }

            //Adds 0 slot for empty slots
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
                 
            }
        }).ToList();

        
        await Task.WhenAll(tasks);}
        catch(Exception ex){
            Console.WriteLine($"Error loading weekly timeslots: {ex.Message}");
        }
    }

    //Handles week changes
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
        if (propertyName == nameof(CurrentWeekRange))
        {
            WeekChanged?.Invoke(this, EventArgs.Empty); // Custom event for the week change
        }
    }
}
