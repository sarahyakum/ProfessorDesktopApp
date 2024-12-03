/*
    Email Students Page:
        Pulls the emails of the students who have either:
            - Not put in any timeslots for the week 
            - Completed the current peer review 

    Written entirely by Emma Hockett for CS 4485.0W1, Started on November 21, 2024
        NetID: ech210001
*/

using CS4485_Team75.ViewModels;
//using CS4485_Team75.Models;
//using CommunityToolkit.Maui.Views;
// using System.Collections.ObjectModel;
namespace CS4485_Team75.Pages;

public partial class EmailStudents : ContentPage
{
    private readonly EmailStudentsViewModel viewModel;

    public EmailStudents(string netid)
    {
        InitializeComponent();
        viewModel = new EmailStudentsViewModel(netid);
        BindingContext = viewModel;
    }

    // Pulls up the list of sections and students immediately upon opening the page 
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is EmailStudentsViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}