namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
using System.Windows.Input;



public class TimeTrackViewModel : INotifyPropertyChanged
{
    private string fileName;
    private string fileContent;

    private DatabaseService databaseService;
    private List<Section> sections;
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand PickAndDisplayFileCommand { get; }

    public List<Section> Sections{
        get => sections;
        set{
            sections=value;
            OnPropertyChanged(nameof(Sections));
        }
        
    }

    public string FileName
    {
        get => fileName;
        set
        {
            if (fileName != value)
            {
                fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
    }
    public string FileContent
    {
        get => fileContent;
        set
        {
            if (fileContent != value)
            {
                fileContent = value;
                OnPropertyChanged(nameof(FileContent));
            }
        }
    }


    public TimeTrackViewModel(string netid)
    {
        PickAndDisplayFileCommand = new Command(async () => await PickAndDisplayFile());

        databaseService = new DatabaseService();
        _ = LoadSectionsAsync(netid);
        

    }

    private async Task LoadSectionsAsync(string netid)
    {
        Sections = await databaseService.GetSections(netid);
        
    }

    private async Task PickAndDisplayFile()
        {
            
            try
            {
                var result = await FilePicker.PickAsync();

                if (result != null)
                {
                
                    // Update the file name property
                    FileName = result.FileName;

                    // Optionally read and update the file content property
                    using var stream = await result.OpenReadAsync();
                    using var reader = new StreamReader(stream);
                    FileContent = await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., user canceled the file picker)
                Console.WriteLine($"File picking error: {ex.Message}");
            }
        }

    
    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
