/*
    View Scores Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: sny200000

*/

using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;
namespace MauiApp1.Pages;


public partial class ViewScoresPopup : ContentPage 
{
    
    private ScoresViewModel viewModel;
    public ObservableCollection<Score> scores { get; set; } = new ObservableCollection<Score>();
    private DatabaseService databaseService;
    public string?  netid; 
    public PeerReview? review;
    public Student? student;
    
    public ViewScoresPopup(string netid, PeerReview review, Student student)
    {
        
        this.netid = netid;
        this.review = review;
        this.student = student;
        
        viewModel = new ScoresViewModel(netid, review, student);
        
        BindingContext = viewModel;
        InitializeComponent();
        databaseService = new DatabaseService();
        //viewModel.PropertyChanged += ViewModel_PropertyChanged;
        
        try{
            _ = InitializeAsync(netid, review, student);
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
        //LoadScoresAndPopulateGrid();
    }
     public async Task LoadDataAsync()
    {
        await viewModel.GetScoresAsync(viewModel.professor_netid, viewModel.sec_code, viewModel.Reviewed.netid, viewModel.review_type);
    
        foreach (var score in viewModel.Scores)
        {
            scores.Add(score);
        }
    }


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
    /*private async void LoadScoresAndPopulateGrid()
{
    await viewModel.GetScoresAsync(viewModel.professor_netid, viewModel.sec_code, viewModel.Reviewed.netid, viewModel.review_type);
    PopulateGrid();
}*/

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ScoresViewModel.Scores))
        {
            PopulateGrid();
        }
    }


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
    }

    

    
}