
/*
	Setting Page: ...

    ADD UPLOAD FILE HERE


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: sny200000


*/
namespace MauiApp1.Pages;

public partial class Settings : ContentPage

{
    private string id;
    public Settings(string netid)
    {
        InitializeComponent();
        id = netid;
    }

    //creating new sections under the professor
    private async void OnAddSectionsButtonClicked(object sender, EventArgs e){
        await DisplayAlert("hellloo", "hi", "OK");
    }

    //adds students to a section
    private async void OnAddStudentsButtonClicked(object sender, EventArgs e){
        await DisplayAlert("hellloo", "hi", "OK");
    }

    //adds students to a team
    private async void OnTeamMembersButtonClicked(object sender, EventArgs e){
        await DisplayAlert("hellloo", "hi", "OK");
    }

    //allows for professor to change password any time
    private async void OnChangePasswordButtonClicked(object sender, EventArgs e){
        await DisplayAlert("hellloo", "hi", "OK");
    }

    
}