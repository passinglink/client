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
    public class GundamCameraViewModel : BaseViewModel {
        public Command SwitchCameraCommand { get; }
        public Command ResetCameraCommand { get; }

        public GundamCameraViewModel() {
            Title = "Gundam Camera";
            SwitchCameraCommand = new Command<string>(async (x) => 
            await ExecuteCameraCommand(x));
            ResetCameraCommand = new Command(async () => await ExecuteResetCameraCommand());
        }

        async Task ExecuteCameraCommand(string x) {
            byte[] val = new byte[] { Byte.Parse(x) };
            Debug.WriteLine($"Camera switching to {x}");
            var result = await DeviceModel.ActiveDevice.GundamCameraChange?.WriteAsync(val, false);
            Debug.WriteLine($"GATT result: {result}");
        }

        async Task ExecuteResetCameraCommand() {
            Debug.WriteLine($"Camera resetting");
            byte[] val = new byte[] { 1 };
            var result = await DeviceModel.ActiveDevice.GundamCameraReset?.WriteAsync(val, false);
        }

        public void OnAppearing() {
        }
    }
}
