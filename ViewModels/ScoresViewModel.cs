/*
    Scores ViewModel Class
        Handles accessing all the scores for that student from their team members

    Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started on November 30, 2024
        NETID: sny200000
*/
namespace CS4485_Team75.ViewModels;

using System.Collections.Specialized;
using System.ComponentModel;
using CS4485_Team75.Models;

public class ScoresViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    
    private List<Score> scores = new List<Score>();
    private Dictionary< string, double> averages = new Dictionary< string, double>();
    public Dictionary< string, double> Averages{
        get => averages;
        set{
            averages = value;
            OnPropertyChanged(nameof(Averages));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public List<Score> Scores{
        get => scores;
        set{
            scores = value;
            OnPropertyChanged(nameof(Scores));
        }
    }
    private Student reviewed;

    public Student Reviewed {
        get => reviewed;
        set{
            reviewed = value;
            OnPropertyChanged(nameof(Reviewed));
        }
    }
    public static string stu_netid = "";
    public string professor_netid;
    public string sec_code;
    public string review_type;
    public string ReviewType {
        get => review_type;
        set{
            review_type = value;
            OnPropertyChanged(nameof(ReviewType));
        }
    }
    public ScoresViewModel(string prof_id, PeerReview review, Student student ){
        databaseService = new DatabaseService();
        reviewed = student;
        stu_netid = student.netid;
        professor_netid = prof_id;
        sec_code = review.section;
        review_type = review.type;
        _ = GetScoresAsync(professor_netid, sec_code, stu_netid, review_type );
        _ = GetAveragesAsync(professor_netid, sec_code, stu_netid, review_type );

    }

    // Begin the process of retrieving the data
    public async Task StartAsync(string prof_id, PeerReview review, Student student ){
        reviewed=student;
        stu_netid = student.netid;
        professor_netid = prof_id;
        sec_code = review.section;
        review_type = review.type;
        await GetScoresAsync(professor_netid, sec_code, stu_netid, review_type );
        await GetAveragesAsync(professor_netid, sec_code, stu_netid, review_type );
    }


    // Calls database to retrieve data
   public async Task GetScoresAsync(string prof_id, string code, string stu_id, string type)
    {
        try
        {
            var reviews = await databaseService.GetReviews(prof_id, code, stu_id, type);
            Scores = reviews ?? new List<Score>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching scores: {ex.Message}");
        }
    }

    //Get Student's average
    public async Task GetAveragesAsync(string prof_id, string code, string stu_id, string type){
        
        try
        {
            var avgs = await databaseService.GetAverages(prof_id, code, stu_id, type);
            Averages = avgs ?? new Dictionary<string, double>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching scores: {ex.Message}");
        }
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}