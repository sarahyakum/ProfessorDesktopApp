using MauiApp1.ViewModels;
using MauiApp1.Models;
namespace MauiApp1.Pages;

public partial class AddSections : ContentPage
{
    string professorID;
    string sectionName;
    string sectionCode;
    DateOnly startdate;
    DateOnly enddate;

    private AddSectionsViewModel viewModel;

    public AddSections(string netid)
    {
        InitializeComponent();
        professorID = netid;
        viewModel = new AddSectionsViewModel(netid);
        BindingContext = viewModel;
    }

    private async void OnAddSectionClicked(object sender, EventArgs e)
    {
        List<string> sectionInfo = new List<string> {NameEntry.Text, CodeEntry.Text};
        string start = StartEntry.Text;
        string end = EndEntry.Text;

        List<DateOnly> dates = new List<DateOnly>{DateOnly.Parse(start), DateOnly.Parse(end)};

        string sectionValidation = await viewModel.AddSectionAsync(professorID, sectionInfo, dates);
        
        if(sectionValidation == "Success"){
            await DisplayAlert("New Section Added.", sectionInfo[0], "OK");
        }
        else{
            await DisplayAlert("Error adding section", sectionValidation, "OK");
        }
    }

}