/*
    Edit Student Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
namespace MauiApp1.Pages.ManageStudents;


public partial class EditStudentPopup : Popup
{
    string studentID;
    public Student studentPassed;
    public EditStudentPopup(Student student)
    {
        InitializeComponent();
        studentPassed = student;
        NameEntry.Text = student.name;
        NetIDEntry.Text = student.netid;
        UTDIDEntry.Text = student.utdid;

        studentID = student.netid;
    }

    // If the changes want to be saved 
    private void OnSaveClicked(object Sender, EventArgs e)
    {
        string updatedName = NameEntry.Text;
        string updatedNetID = NetIDEntry.Text;
        string updatedUTDID = UTDIDEntry.Text;

        Close(new Student
        {
            name = updatedName,
            netid = updatedNetID,
            utdid = updatedUTDID
        });
    }

    // If they choose their mind about editing, returns the same student
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(studentPassed);
    }

}