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
	public DateTime Timestamp { get; set; }

	private StudentSheetsViewModel viewModel;
	public ObservableCollection<WorkHoursEntry> WorkHours { get; set; }

	public Timesheets(string className, string code)
	{
		viewModel = new StudentSheetsViewModel(code);
		BindingContext = viewModel;
		InitializeComponent();
		DisplayTime();
		SectionName.Text = className;
		// Sample data
       WorkHours = new ObservableCollection<WorkHoursEntry>
        {
            new WorkHoursEntry { StudentName = "Alice", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Bob", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Charlie", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Diana", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Evan", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Fiona", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "George", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Holly", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Ian", HoursByDate = GenerateSampleHours() },
            new WorkHoursEntry { StudentName = "Jane", HoursByDate = GenerateSampleHours() }
        };
	
		PopulateGrid();
		

	}
	private Dictionary<string, string> GenerateSampleHours()
    {
        var hoursByDate = new Dictionary<string, string>();
        for (int day = 1; day <= 14; day++) // Adding 14 dates to ensure horizontal scrolling
        {
            string date = $"2024-11-{day:00}";
            hoursByDate[date] = new Random().Next(0, 8).ToString(); // Random hours between 0 and 8
        }
        return hoursByDate;
    }
	private void PopulateGrid()
    {
        // Extract all unique dates across all students for column headers
        var allDates = WorkHours
            .SelectMany(student => student.HoursByDate.Keys)
            .Distinct()
            .OrderBy(date => date)
            .ToList();

        // Add dates as column headers in the first row
        for (int columnIndex = 1; columnIndex <= allDates.Count; columnIndex++)
        {
            WorkHoursGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            var dateLabel = new Label
            {
                Text = allDates[columnIndex - 1],
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            WorkHoursGrid.Add(dateLabel, columnIndex, 0); // Row 0, dynamic column
        }

        // Add students and their hours worked as rows
        for (int rowIndex = 1; rowIndex <= WorkHours.Count; rowIndex++)
        {
            var student = WorkHours[rowIndex - 1];

            // Add student name as row header
            WorkHoursGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            var nameLabel = new Label
            {
                Text = student.StudentName,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };
            WorkHoursGrid.Add(nameLabel, 0, rowIndex); // Column 0, dynamic row

            // Add bordered buttons for hours worked, allowing editing on click
            for (int columnIndex = 1; columnIndex <= allDates.Count; columnIndex++)
            {
                var date = allDates[columnIndex - 1];
                var hours = student.HoursByDate.ContainsKey(date) ? student.HoursByDate[date] : "0";

                var button = new Button
                {
                    Text = hours,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 60,  // Set cell width
                    HeightRequest = 40  // Set cell height
                };

                // Button click event to open an edit prompt
                button.Clicked += async (sender, e) =>
                {
                    string result = await DisplayPromptAsync("Edit Hours", $"Enter hours for {student.StudentName} on {date}", 
                                                              initialValue: button.Text, 
                                                              maxLength: 3, 
                                                              keyboard: Keyboard.Numeric);
                    
                    if (result != null)
                    {
                        button.Text = result; // Update button label
                        student.HoursByDate[date] = result; // Update the data model
                    }
                };

                // Wrap button in a Frame to create a border effect
                var borderedCell = new Frame
                {
                    Content = button,
                    Padding = 2,
                    Margin = 1,
                    BorderColor = Colors.Gray,
                    CornerRadius = 5,
                    HasShadow = false,
                    WidthRequest = 60,
                    HeightRequest = 40
                };

                WorkHoursGrid.Add(borderedCell, columnIndex, rowIndex); // Dynamic column, dynamic row
            }
        }
    }

	
	//displays time can use datetime to keep up with current date
	private void DisplayTime(){
		
		Time.Text = DateTime.Now.ToString("G");
	}

	//Allows for us to view the students and their timesheets in the section

	private async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e){
		// Inside any page
		var student = e.SelectedItem as Student;
		string netid = student.netid;


	}
}