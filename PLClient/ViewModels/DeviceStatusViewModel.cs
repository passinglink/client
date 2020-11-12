using PLClient.Models;
using PLClient.Views;
using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PLClient.ViewModels {
    public class DeviceStatusViewModel : BaseViewModel {
        public string Name { get => DeviceModel.ActiveDevice.Name; }
        public string Id { get => DeviceModel.ActiveDevice.Id; }

        // TODO: There has to be a better way to do this...
        bool _connected = false;
        public bool Connected {
            get => _connected;
            set {
                Debug.WriteLine($"Device connection status = {value}");
                _connected = value;
                OnPropertyChanged("Connected");
                OnPropertyChanged("ConnectButtonText");
                OnPropertyChanged("StatusText");
            }
        }

        public string ConnectButtonText {
            get => Connected ? "Disconnect" : "Connect";
        }

        public string StatusText {
            get => Connected ? "Connected" : "Disconnected";
        }

        public Command ConnectButtonCommand { get; }

        public DeviceStatusViewModel() {
            Title = "Device Status";
            Connected = DeviceModel.ActiveDevice.Handle.IsConnected();
            ConnectButtonCommand = new Command(async () => await ExecuteConnectCommand());
        }

        bool _connectButtonPressed = false;
        static int counter = 0;
        async Task ExecuteConnectCommand() {
            if (Device.RuntimePlatform == Device.UWP) {
                // HACK: https://github.com/xamarin/Xamarin.Forms/issues/10136
                if (_connectButtonPressed) {

                    _connectButtonPressed = false;
                    return;
                }
                _connectButtonPressed = true;
            }

            Debug.WriteLine($"Counter = {counter++}");
            if (!_connected) {
                Debug.WriteLine($"Connecting to device {DeviceModel.ActiveDevice.Id}");
                Connected = await DeviceModel.ActiveDevice.Connect();
            } else {
                Debug.WriteLine($"Disconnecting from device {DeviceModel.ActiveDevice.Id}");
                DeviceModel.ActiveDevice.Disconnect();
                Connected = false;
            }
        }

        public void OnAppearing() {
        }
    }
}
