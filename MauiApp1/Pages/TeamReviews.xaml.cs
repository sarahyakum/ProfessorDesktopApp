
using MauiApp1.ViewModels;
namespace MauiApp1.Pages;


public partial class TeamReviews : ContentPage
{
	

	private ReviewViewModel viewModel;

	public TeamReviews(string className, string code)
	{
		InitializeComponent();
        viewModel = new ReviewViewModel(code);
		BindingContext = viewModel;
		SectionName.Text = className;
		

	}

	
}