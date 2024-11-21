/*
    Edit Student Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered

    Written by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;
namespace MauiApp1.Pages.ManageStudents;


public partial class EditStudentPopup : Popup
{
    string studentID;
    public Student studentPassed;
    private readonly ManageStudentsViewModel viewModel;
    public EditStudentPopup(ManageStudentsViewModel viewModel, Student student)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        studentPassed = student;
        NameEntry.Text = student.name;
        NetIDEntry.Text = student.netid;
        UTDIDEntry.Text = student.utdid;

        studentID = student.netid;
    }

    // If the changes want to be saved 
    private async void OnSaveClicked(object Sender, EventArgs e)
    {
        
        // Checks whether all of the fields are filled out 
        if(string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(NetIDEntry.Text) || string.IsNullOrWhiteSpace(UTDIDEntry.Text) )
        {
            ErrorLabel.Text = "All fields must be filled out to save changes.";
            return;
        }
        else if(studentPassed.name == NameEntry.Text && studentPassed.netid == NetIDEntry.Text && studentPassed.utdid == UTDIDEntry.Text)
        {
            // If nothing was changed return the same student 
            Close(studentPassed);
        }

        // Gathering the information 
        string updatedName = NameEntry.Text;
        string updatedNetID = NetIDEntry.Text;
        string updatedUTDID = UTDIDEntry.Text;
        List<string> updatedInfo = new List<string> {updatedNetID, updatedName, updatedUTDID};


        // Editing the student and returning whether it worked 
        string editValidation = await viewModel.EditStudentAsync(studentID, updatedInfo);

        if(editValidation == "Success")
        {
            Close(new Student
            {
                name = updatedName,
                netid = updatedNetID,
                utdid = updatedUTDID
            });
        }
        else{
            ErrorLabel.Text = editValidation;
            return;
        }
    }

    // If they choose their mind about editing, returns the same student
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close(studentPassed);
    }

}