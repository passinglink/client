using PLClient.Models;
using PLClient.Views;
using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PLClient.ViewModels {
    public class DeviceListViewModel : BaseViewModel {
        readonly IBleManager ble;

        public ObservableCollection<DeviceModel> Devices { get; }
        public Command ScanButtonCommand { get; }
        public Command<DeviceModel> DeviceTapped { get; }

        public DeviceListViewModel() {
            ble = (IBleManager)ShinyHost.Container.GetService(typeof(IBleManager));

            Title = "Devices";
            Devices = new ObservableCollection<DeviceModel>();
            ScanButtonCommand = new Command(async () => await ExecuteScanButtonCommand());

            DeviceTapped = new Command<DeviceModel>(OnDeviceSelected);
        }

        void RestartScan() {
            Debug.WriteLine("Scanning restarted");
            Devices.Clear();
            ble.StopScan();
            ble.ScanForUniquePeripherals().Subscribe(async device => {
                // TODO: Actually look at advertising data.
                if (device.Name == "Passing Link") {
                    Debug.WriteLine($"Found device: {device.Uuid}");
                    Devices.Add(new DeviceModel { Name = device.Name, Id = device.Uuid, Handle = device });
                }
            });
        }

        private bool _scanButtonPressed = false;
        async Task ExecuteScanButtonCommand() {
            if (Device.RuntimePlatform == Device.UWP) {
                // HACK: https://github.com/xamarin/Xamarin.Forms/issues/10136
                if (_scanButtonPressed) {
                    _scanButtonPressed = false;
                    return;
                }
                _scanButtonPressed = true;
            }

            RestartScan();
        }

        public void OnAppearing() {
            RestartScan();
        }

        async void OnDeviceSelected(DeviceModel device) {
            if (device == null)
                return;

            DeviceModel.ActiveDevice = device;
            await Shell.Current.GoToAsync($"///{nameof(DeviceStatusPage)}");
            //await Shell.Current.GoToAsync($"{nameof(DevicePage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}