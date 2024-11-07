namespace MauiApp1.ViewModels;

using System.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Pages;

public class StudentSheetsViewModel : INotifyPropertyChanged
{
    private DatabaseService databaseService;
    private List<Student> students = new List<Student>();
    public event PropertyChangedEventHandler PropertyChanged;
    
    public List<Student> Students{
        get => students;
        set{
            students=value;
            OnPropertyChanged(nameof(Students));
        }
        
        }

    public StudentSheetsViewModel(string sectionCode)
    {
        databaseService = new DatabaseService();
        LoadStudentsAsync(sectionCode);

    }

    private async void LoadStudentsAsync(string code)
    {
        Students = await databaseService.GetStudents(code);
        List<Student> new_students=new List<Student>();
        foreach (Student student in Students){
            string netid = student.netid;
            Student stu = await databaseService.GetStudentsInfo(netid);
            new_students.Add(stu);

        }
        Students = new_students;
        
        
    }

    protected virtual void OnPropertyChanged( string propertyName )  {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
