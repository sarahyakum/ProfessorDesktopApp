
using CommunityToolkit.Maui.Views;
using MauiApp1.Models;
using MauiApp1.ViewModels;

namespace MauiApp1.Pages;

    public partial class PopupPage : Popup
    {
        //List<Timeslot> slots = new List<Timeslot>();

        private TimeslotViewModel viewModel;
        public PopupPage(string id)
        {
            InitializeComponent();
            viewModel = new TimeslotViewModel(id);
            BindingContext = viewModel;

        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }
    }

