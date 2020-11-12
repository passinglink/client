using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class InputForwardingPage : ContentPage {
        InputForwardingViewModel _viewModel;

        public InputForwardingPage() {
            InitializeComponent();

            BindingContext = _viewModel = new InputForwardingViewModel();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}