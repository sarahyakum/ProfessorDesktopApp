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
	public DateOnly Timestamp { get; set; }
    public string section;
    public ObservableCollection<Student> students { get; set; } = new ObservableCollection<Student>();
	private TimesheetsViewModel viewModel;
	
    public async Task LoadDataAsync()
        {
            await viewModel.GetTimeFrameAsync(section);
            foreach (var student in viewModel.Students)
            {
                students.Add(student);
            }
        }
	public Timesheets(string className, string code)
	{
        section = code;
		viewModel = new TimesheetsViewModel(code);
		BindingContext = viewModel;
		InitializeComponent();
		
		SectionName.Text = className;
        try
        {
            _ = InitializeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in InitializeAsync: {ex.Message}");
        }
        
		

	}
    private async Task InitializeAsync()
    {
       try
    {
        await viewModel.StartAsync(section);
        await LoadDataAsync();
        PopulateGrid(); // Ensure this doesn't throw exceptions
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during initialization: {ex.Message}");
    }
    }
	private void PopulateGrid()
    {
        WorkHoursGrid.Clear();
        
        //WorkHoursGrid.RowDefinitions.Clear(); // Clear previous rows
        //WorkHoursGrid.ColumnDefinitions.Clear(); // Clear previous columns

        WorkHoursGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        WorkHoursGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        var weekStart = viewModel.CurrentWeekStart;
        var weekDates = Enumerable.Range(0, 7).Select(offset => weekStart.AddDays(offset)).ToList();

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
            WorkHoursGrid.Add(date, columnIndex, 0); // Row 0, dynamic column
        }

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
            WorkHoursGrid.Add(name, 0, rowIndex); // Column 0, dynamic row

            // Add bordered buttons for hours worked, allowing editing on click
            for (int columnIndex = 1; columnIndex <= weekDates.Count; columnIndex++)
            {
                var date = weekDates[columnIndex - 1];
                var hours = student.timeslots[date].hours;
                //var hours = student.timeslots.ContainsKey(date) ? student.timeslots[date] : "0";
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
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 100,  // Set cell width
                    HeightRequest = 100  // Set cell height
                };

                // Button click event to open an edit prompt
                button.Clicked += async (sender, e) =>
                {
                    string result = await DisplayPromptAsync("Edit Hours for: "+student.timeslots[date].description,  $"\nEnter hours for {student.netid} on {date}", 
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
                // Wrap button in a Frame to create a border effect
                var borderedCell = new Frame
                {
                    Content = button,
                    Padding = 10,  // Add padding inside the cell
                    BorderColor = Colors.Black,
                    //BackgroundColor = Colors.LightGray,
                    WidthRequest = 120,
                    HeightRequest = 120
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

