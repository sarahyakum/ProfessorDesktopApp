/*
    View Score Page:
        Shows table for selected student's peer review scores from team members
        Allows for professor to update the score if need be

   Written by Sarah Yakum (sny200000) for CS 4485.0W1 Started on December 1, 2024 


*/

using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
using System.Collections.ObjectModel;
namespace CS4485_Team75.Pages;


public partial class ViewScores : ContentPage 
{
    
    private ScoresViewModel viewModel;
    private DatabaseService databaseService;
    public string?  netid; 
    public PeerReview? review;
    public Student? student;
    public ObservableCollection<Score> scores { get; set; } = new ObservableCollection<Score>();
    public Dictionary< string, double> averages = new Dictionary< string, double>();


    
    public ViewScores(string netid, PeerReview review, Student student)
    {
        
        this.netid = netid;
        this.review = review;
        this.student = student;
        
        
        viewModel = new ScoresViewModel(netid, review, student);
        
        BindingContext = viewModel;
        InitializeComponent();
        databaseService = new DatabaseService();
        
        //viewModel.PropertyChanged += ViewModel_PropertyChanged;
        
        // Begin obtaining the data to show
        try{
            _ = InitializeAsync(netid, review, student);
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
        //LoadScoresAndPopulateGrid();
    }

    // Calls database to get student's peer review scores
     public async Task LoadDataAsync()
    {
        await viewModel.GetScoresAsync(viewModel.professor_netid, viewModel.sec_code, viewModel.Reviewed.netid, viewModel.review_type);
        await viewModel.GetAveragesAsync(viewModel.professor_netid, viewModel.sec_code, viewModel.Reviewed.netid, viewModel.review_type);
    
        foreach (var score in viewModel.Scores)
        {
            scores.Add(score);
        }
        foreach(var avg in viewModel.Averages){
            averages[avg.Key] = avg.Value;

        }
    }

    // Begins the process of obtaining data when called
    private async Task InitializeAsync(string netid, PeerReview review, Student student){
        try{
            scores.Clear();
            await viewModel.StartAsync(netid, review, student);
            await LoadDataAsync();
            PopulateGrid();
        }
        catch (Exception ex){
            Console.WriteLine(ex.Message);
        }  
    }
    
    //Handles changes to the data
    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ScoresViewModel.Scores))
        {
            PopulateGrid();
        }
    }

    // Creates the table view for the student's Peer Review results
    private void PopulateGrid(){
        ScoreGrid.Clear();
        ScoreGrid.ColumnDefinitions.Clear();
        ScoreGrid.RowDefinitions.Clear();
        ScoreGrid.Children.Clear();
        var criterias = viewModel.Scores
            .Select(s => s.criteria)
            .Distinct()
            .ToList();

        var students = viewModel.Scores
            .Select(s => s.reviewer)
            .Distinct()
            .ToList();
        int y = 1;
        foreach(var criteria in criterias){
            
            ScoreGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            var header = new Label
            {
                Text = criteria,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            
            ScoreGrid.Add(header, y, 0); 
            y++;

        }
        int x = 1;
        foreach(var student in students){
            
            ScoreGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var nameLabel = new Label
            {
                Text = student,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };
            
            ScoreGrid.Add(nameLabel, 0, x);
            
            int yIndex = 1;

            foreach(var criteria in criterias){
                var score = scores.FirstOrDefault(s => s.reviewer == student && s.criteria == criteria);
                var scoreEntry = new Entry
                {
                    Text = score?.score?.ToString(), // Display existing score if available
                    HorizontalTextAlignment = TextAlignment.Center,
                    Keyboard = Keyboard.Numeric
                };
                //scoreEntry.BindingContext = new { Reviewer = student, Criteria = criteria };
                scoreEntry.TextChanged += (sender, e) =>
                {
                    int.TryParse(e.NewTextValue, out int newScore);

                    // Update the score in the view model
                    var existingScore = viewModel.Scores
                        .FirstOrDefault(s => s.reviewer == student && s.criteria == criteria);

                    if (existingScore != null)
                    {
                        existingScore.score = newScore;
                    }
                    else
                    {
                        // If no score exists, create a new entry
                        viewModel.Scores.Add(new Score
                        {
                            reviewer = student,
                            criteria = criteria,
                            score = newScore
                        });
                    }
                };
                
                

                ScoreGrid.Add(scoreEntry, yIndex, x); // Add Entry to the grid
                yIndex++;
            }
            
            x++;
            
            
            


        }
        int z = 1;
            foreach(var avg in averages){
                var avgValue = new Label
                {
                    Text = avg.Value.ToString(),
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
                
                ScoreGrid.Add(avgValue, z++, x);

            }
        var avgLabel = new Label
            {
                Text = "Averages",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };
        ScoreGrid.Add(avgLabel, 0, x);
    }
    
}