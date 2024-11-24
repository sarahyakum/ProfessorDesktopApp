/*
    Email Students Page:
        Pulls the emails of the students who have either:
            - Not put in any timeslots for the week 
            - Completed the current peer review 

    Written entirely by Emma Hockett for CS 4485.0W1, Started on November 21, 2024
        NetID: ech210001
*/

using MauiApp1.ViewModels;
using MauiApp1.Models;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
namespace MauiApp1.Pages;

public partial class EmailStudents : ContentPage
{
    private readonly EmailStudentsViewModel viewModel;

    public EmailStudents(string netid)
    {
        InitializeComponent();
        viewModel = new EmailStudentsViewModel(netid);
        BindingContext = viewModel;
    }
}