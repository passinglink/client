using PLClient.Models;
using PLClient.Services;
using PLClient.Views;
using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PLClient.ViewModels {
    public class InputViewModel : BaseViewModel {
        public Command<string> InputCommand { get; }

        public InputViewModel() {
            InputCommand = new Command<string>(async (x) => ExecuteInputCommand(x));
        }

        int buttonToOffset(string button) {
            switch (button) {
                case "up":
                    return 14;
                case "down":
                    return 15;
                case "left":
                    return 17;
                case "right":
                    return 16;

                case "select":
                    return 10;
                case "home":
                    return 12;
                case "start":
                    return 11;

                case "north":
                    return 0;
                case "south":
                    return 2;
                case "west":
                    return 3;
                case "east":
                    return 1;
            }

            return -1;
        }
        async Task ExecuteInputCommand(string button) {
            Debug.WriteLine($"Button: {button}: offset = {buttonToOffset(button)}");

            int x = 1 << buttonToOffset(button);
            var bytes = BitConverter.GetBytes(x);

            if (DeviceModel.ActiveDevice == null) {
                Debug.WriteLine("No active device");
                return;
            }

            if (DeviceModel.ActiveDevice.SetInput == null) {
                Debug.WriteLine("Device doesn't have input service");
                return;
            }

            await DeviceModel.ActiveDevice.SetInput.WriteAsync(bytes, false);
            await Task.Delay(30);
            await DeviceModel.ActiveDevice.SetInput.WriteAsync(BitConverter.GetBytes(0), false);
        }

        public void OnAppearing() {
        }
    }
}
