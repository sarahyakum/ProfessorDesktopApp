/*
	Setting Page: MAYBE WE NEED TO CHANGE THIS NAME?
		NOT SURE WHAT THIS ONE DOES YET


	Written by Sarah Yakum for CS 4485.0W1, Senior Design Project, Started in ...
        NetID: ...


*/
using MauiApp1.ViewModels;
namespace MauiApp1.Pages;

public partial class Settings : ContentPage
{
	string professorID;
	string section;
	string type;
	//List<string> criteriaInfo;
	private PRSetUpViewModel viewModel;

	public Settings(string netid)
	{
		InitializeComponent();
		professorID = netid;
		viewModel = new PRSetUpViewModel();
	}


	private async void OnCriteriaClicked(object sender, EventArgs e){
		List<string> criteriaInfo = new List<string>();
		criteriaInfo.Add(CodeEntry.Text);
		criteriaInfo.Add(CriteriaEntry.Text);
		criteriaInfo.Add(DescriptionEntry.Text);
		criteriaInfo.Add(TypeEntry.Text);
		string criteriaValidation = await viewModel.CriteriaAsync(professorID, criteriaInfo);
		if (criteriaValidation == "Success"){
			await DisplayAlert("New Criteria Added." , criteriaInfo[1], "OK");
		}
		else{

			await DisplayAlert("Error adding Criteria", criteriaValidation, "OK");
			
		}
	
	}
	private async void OnCreatePeerReviewClicked(object sender, EventArgs e){
		await DisplayAlert("hellloo", "hi", "OK");
		
	}




}
