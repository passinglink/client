using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class DeviceStatusPage : ContentPage {
        DeviceStatusViewModel _viewModel;

        public DeviceStatusPage() {
            InitializeComponent();

            BindingContext = _viewModel = new DeviceStatusViewModel();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}