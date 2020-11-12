using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class GundamCameraPage : ContentPage {
        GundamCameraViewModel _viewModel;

        public GundamCameraPage() {
            InitializeComponent();

            BindingContext = _viewModel = new GundamCameraViewModel();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}