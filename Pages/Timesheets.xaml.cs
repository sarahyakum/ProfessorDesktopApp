/*
	Timesheets Page:
		Displays the table for each students time sheet in a section.
        Also shows the total time logged to dat by the student's name

	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on October 14, 2024
        NetID: sny200000 
*/

using CS4485_Team75.ViewModels;
using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using System.Collections.ObjectModel;

namespace CS4485_Team75.Pages;


public partial class Timesheets : ContentPage
{
	public string section;
   	private TimesheetsViewModel viewModel;
    private DatabaseService databaseService;
    public DateOnly Timestamp { get; set; }
    public ObservableCollection<Student> students { get; set; } = new ObservableCollection<Student>();

	public Timesheets(string className, string code)
	{
        // Get binding context from view model
        section = code;
		viewModel = new TimesheetsViewModel(code);
		BindingContext = viewModel;
		InitializeComponent();
		SectionName.Text = className;
        databaseService = new DatabaseService();
        
        // Moving from week to week
        viewModel.WeekChanged +=  (sender, args) => {
            
            try{
                _ = InitializeAsync();
            }
            catch (Exception ex){
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        };

        try{
            _ = InitializeAsync();
        }
        catch (Exception ex){
            Console.WriteLine($"Error in InitializeAsync: {ex.Message}");
        }
        
		

	}
    //Handles refresh of page
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is TimesheetsViewModel)
        {
            await InitializeAsync();
        }
    }
    // Gets all the students time slot data
    public async Task LoadDataAsync()
    {
        await viewModel.GetTimeFrameAsync(section);
        foreach (var student in viewModel.Students)
        {
            students.Add(student);
        }
    }

    //Starts the view models tasks
    private async Task InitializeAsync()
    {
       try{
        students.Clear();
        await viewModel.StartAsync(section);
        await LoadDataAsync();
        PopulateGrid(); 
        }
        catch (Exception ex){
            Console.WriteLine($"Error during initialization: {ex.Message}");
        }
    }
	
    //Creates table of students weekly timesheet
    private void PopulateGrid()
    {
        WorkHoursGrid.Clear();
        
        WorkHoursGrid.RowDefinitions.Clear(); 
        WorkHoursGrid.ColumnDefinitions.Clear(); 

        WorkHoursGrid.Children.Clear();

        WorkHoursGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        WorkHoursGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        var weekStart = viewModel.CurrentWeekStart;
        var weekDates = Enumerable.Range(0, 7).Select(offset => weekStart.AddDays(offset)).ToList();
        
        //sets up the column headers
        for (int columnIndex = 1; columnIndex <= weekDates.Count; columnIndex++)
        {
            WorkHoursGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            
            
            var dateLabel = new Label
            {
                Text = weekDates[columnIndex - 1].ToString(),
                FontAttributes = FontAttributes.Bold,
                FontSize= 20,
                HorizontalOptions = LayoutOptions.Center
            };
            var date = new Frame{
                Content = dateLabel,
                BorderColor = Colors.Black


            };
            WorkHoursGrid.Add(dateLabel, columnIndex, 0); // Row 0, dynamic column
        }
        //Set up row headers and data in rows
        for (int rowIndex = 1; rowIndex <= students.Count; rowIndex++)
        {
            var student = students[rowIndex - 1];

            // Add student name as row header
            WorkHoursGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var total = student.timeslots?.FirstOrDefault().Value?.total;
            var nameLabel = new Label
            {
                Text = student.name + " - "+ total,
                FontAttributes = FontAttributes.Bold,
                FontSize= 20,
                VerticalOptions = LayoutOptions.Center
            };
            var name = new Frame{
                Content = nameLabel,
                BorderColor = Colors.Black


            };
            WorkHoursGrid.Add(nameLabel, 0, rowIndex); // Column 0, dynamic row

            
            for (int columnIndex = 1; columnIndex <= weekDates.Count; columnIndex++)
            {
                var date = weekDates[columnIndex - 1];
                var hours = student?.timeslots?[date]?.hours;
                if(student?.timeslots == null){
                    return;
                }
                if (!student.timeslots.ContainsKey(date))
                {
                    student.timeslots[date] = new Timeslot
                    {
                        date = date,
                        hours = "00:00",
                        description = "Given student description on this date"
                    };
                }
                var timeslot = student.timeslots[date];
                var button = new Button
                {
                    Text = hours?.ToString(),
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    //BackgroundColor = Colors.White,
                    //BorderColor = Colors.Black,
                    WidthRequest = 95,  // Set cell width
                    HeightRequest = 95  // Set cell height
                };

                //Handles updating students time logged
                button.Clicked += async (sender, e) =>
                {
                    string result = await DisplayPromptAsync($"Edit Hours for: \"{student.timeslots[date].description}\"",  $"\nEnter hours for {student.netid} on {date}", 
                                                              initialValue: button.Text, 
                                                              maxLength: 5, 
                                                              keyboard: Keyboard.Numeric);
                    
                    if (result != null)
                    {
                        if(student.timeslots[date].description == null){
                            student.timeslots[date].description = "Given student description on this date";
                        }
                        string text = student.timeslots[date].description?? "Given student description on this date";
                        string change = await UpdateSlot(student.netid, date, text, result);
                        if (change != "success"){
                            await DisplayAlert("Error", "Please try again later.", "OK");
                            

                        }
                        else{
                            button.Text = result;
                            student.timeslots[date] = new Timeslot{
                                date = date,
                                hours = result,
                                description = student.timeslots[date].description 
                            };

                        }
                        

                    };

                };
               
                var borderedCell = new Frame
                {
                    Content = button,
                    WidthRequest = 100,
                    HeightRequest = 100
                };

                WorkHoursGrid.Add(borderedCell, columnIndex, rowIndex); // Dynamic column, dynamic row
            }
        }
    }
       
       
       
    // Handles input from the professor to update student's timeslot
    public async Task<string> UpdateSlot(string netid, DateOnly date, string desc, string dur){
        string result = await databaseService.EditTimeslot(netid, date, desc, dur);
        if(result != "Success"){
            await DisplayAlert("System Error", result, "OK" );
            return "error";
        }
        return "success";
    }

}

