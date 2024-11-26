
/*
	Setting Page: ...

    Allows the professor to manage sections, students, teams, or change their password


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000


*/
using MauiApp1.Pages;
using MauiApp1.Pages.ManageSectionPages;
namespace MauiApp1.Pages;

public partial class Settings : ContentPage

{
    private readonly string id;
    public Settings(string netid)
    {
        InitializeComponent();
        id = netid;
    }

    //creating new sections under the professor
    private async void OnManageSectionsButtonClicked(object sender, EventArgs e){
        await Navigation.PushAsync(new ManageSectionPages.ManageSections(id));
    }

    //Adds students to a section, Directs them to choose the setion first 
    private async void OnManageStudentsButtonClicked(object sender, EventArgs e){
        string flag="ADDSTU";
        await Navigation.PushAsync(new Sections(id, flag));
    }

    //Adds students to a team, Directs them to choose the section first 
    private async void OnManageTeamsButtonClicked(object sender, EventArgs e){
        string flag="TEAM";
        await Navigation.PushAsync(new Sections(id, flag));
    }

    // Allows for professor to change password any time
    private async void OnChangePasswordButtonClicked(object sender, EventArgs e){
        await Navigation.PushAsync(new ChangePassword(id));
    }
    
}