using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class InputPage : ContentPage {
        InputViewModel _viewModel;

        public InputPage() {
            InitializeComponent();

            BindingContext = _viewModel = new InputViewModel();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}