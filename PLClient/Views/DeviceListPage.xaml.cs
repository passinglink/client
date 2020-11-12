using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class DeviceListPage : ContentPage {
        DeviceListViewModel _viewModel;

        public DeviceListPage() {
            InitializeComponent();

            BindingContext = _viewModel = new DeviceListViewModel();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}