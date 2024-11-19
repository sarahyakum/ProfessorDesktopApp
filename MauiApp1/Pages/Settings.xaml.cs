
/*
	Setting Page: ...

    Allows the professor to manage sections, students, teams, or change their password


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000


*/
namespace MauiApp1.Pages;

public partial class Settings : ContentPage

{
    private string id;
    private string flag;
    public Settings(string netid)
    {
        InitializeComponent();
        id = netid;
    }

    //creating new sections under the professor
    private async void OnManageSectionsButtonClicked(object sender, EventArgs e){
        await Navigation.PushAsync(new AddSections(id));
    }

    //adds students to a section
    private async void OnManageStudentsButtonClicked(object sender, EventArgs e){
        flag="ADDSTU";
        await Navigation.PushAsync(new Sections(id, flag));
    }

    //adds students to a team
    private async void OnManageTeamsButtonClicked(object sender, EventArgs e){
        flag="TEAM";
        await Navigation.PushAsync(new Sections(id, flag));
    }

    //allows for professor to change password any time
    private async void OnChangePasswordButtonClicked(object sender, EventArgs e){
        await Navigation.PushAsync(new ChangePassword(id));
    }

    
}