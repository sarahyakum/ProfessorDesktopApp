using System.Collections.ObjectModel;


namespace MauiApp1.Pages;

public partial class Test : ContentPage
{
    // Observable collection to hold the list of students
    public ObservableCollection<Student1> Students { get; set; }

    public Test()
    {
        InitializeComponent();

        // Initialize the Students collection with sample data
        Students = new ObservableCollection<Student1>
        {
            new Student1 { Name = "Alice Johnson", Age = 20, Class = "Math 101" },
            new Student1 { Name = "Bob Smith", Age = 22, Class = "Physics 202" },
            new Student1 { Name = "Catherine Brown", Age = 21, Class = "History 101" }
        };

        // Set the BindingContext to the current page
        BindingContext = this;
    }
}

// Define a simple Student model class
public class Student1
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Class { get; set; }
}

