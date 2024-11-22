/*
	Timesheets Page:
		Displays the Time sheets FOR A SPECIFIC SECTION


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ... ADD THIS 
        NetID: sny200000 
*/

using MauiApp1.ViewModels;
using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using System.Collections.ObjectModel;

namespace MauiApp1.Pages;


public partial class Timesheets : ContentPage
{
	public string section;
    public DateOnly Timestamp { get; set; }
   	private TimesheetsViewModel viewModel;
    public ObservableCollection<Student> students { get; set; } = new ObservableCollection<Student>();


	public Timesheets(string className, string code)
	{
        //get binding context from view model
        section = code;
		viewModel = new TimesheetsViewModel(code);
		BindingContext = viewModel;
		InitializeComponent();
		SectionName.Text = className;
        
        //Moving from week to week
        viewModel.WeekChanged += async (sender, args) => {
            try{
                
                await LoadDataAsync();
                PopulateGrid(); 
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
    //Get all the students time slot data
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
            var nameLabel = new Label
            {
                Text = student.netid,
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
                var hours = student.timeslots[date].hours;
                
                if (!student.timeslots.ContainsKey(date))
                {
                    student.timeslots[date] = new Timeslot
                    {
                        date = date,
                        hours = "00:00",
                        description = "No entry"
                    };
                }
                var button = new Button
                {
                    Text = hours.ToString(),
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 70,  // Set cell width
                    HeightRequest = 70  // Set cell height
                };

                //THIS NEEDS TO BE CONNECTED TO THE DB
                button.Clicked += async (sender, e) =>
                {
                    string result = await DisplayPromptAsync($"Edit Hours for: \"{student.timeslots[date].description}\"",  $"\nEnter hours for {student.netid} on {date}", 
                                                              initialValue: button.Text, 
                                                              maxLength: 5, 
                                                              keyboard: Keyboard.Numeric);
                    
                    if (result != null)
                    {
                        button.Text = result; // Update button label
                        student.timeslots[date] = new Timeslot{
                            date = date,
                            hours = result,
                            description = student.timeslots[date].description // Update the data model
                    };
                };

               };
               
                var borderedCell = new Frame
                {
                    Content = button,
                    //Padding = 10,  
                    BorderColor = Colors.Black,
                    //BackgroundColor = Colors.LightGray,
                    WidthRequest = 75,
                    HeightRequest = 75
                };

                WorkHoursGrid.Add(borderedCell, columnIndex, rowIndex); // Dynamic column, dynamic row
            
        }}
        }
       
       
       

	private async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e){
		// Inside any page
		var student = e.SelectedItem as Student;
		string netid = student.netid;


	}

}

