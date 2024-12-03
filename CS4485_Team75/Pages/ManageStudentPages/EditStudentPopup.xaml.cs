/*
    Edit Student Popup:
        From the Manage Sections Page when the professor chooses to edit a section 
        Shows a pop up with the current information in text input fields where it can be altered

    Written entirely by Emma Hockett for CS 4485.0W1 Senior Design Prject, Started on November 19, 2024
        NetID: ech210001

*/

using CommunityToolkit.Maui.Views;
using CS4485_Team75.Models;
using CS4485_Team75.ViewModels;
namespace CS4485_Team75.Pages.ManageStudents;


public partial class EditStudentPopup : Popup
{
    readonly string studentID;
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

    // If the professor attempts to save the changes made 
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
            Close();
        }


        string updatedName = NameEntry.Text;
        string updatedNetID = NetIDEntry.Text;
        string updatedUTDID = UTDIDEntry.Text;
        List<string> updatedInfo = new List<string> {updatedNetID, updatedName, updatedUTDID};


        // Editing the student, if it was not able to, displays the reason why 
        string editValidation = await viewModel.EditStudentAsync(studentID, updatedInfo);

        if(editValidation == "Success")
        {
            Close();
        }
        else{
            ErrorLabel.Text = editValidation;
            return;
        }
    }

    // If they choose their mind about editing closes the popup
    private void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}