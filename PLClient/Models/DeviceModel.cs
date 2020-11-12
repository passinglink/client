using Shiny.BluetoothLE;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PLClient.Models {
    public class DeviceModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public IPeripheral Handle { get; set; }

        public async Task<bool> Connect() {
            await DeviceModel.ActiveDevice.Handle.ConnectWaitAsync();
            if (!DeviceModel.ActiveDevice.Handle.IsConnected()) {
                return false;
            }

            var services = await DeviceModel.ActiveDevice.Handle.GetServicesAsync();
            
            foreach (var service in services) {
                Debug.WriteLine($"service = {service.Uuid}");
                if (service.Uuid == INPUT_SERVICE_UUID) {
                    Debug.WriteLine("Found input forwarding service");
                    var characteristics = await service.GetCharacteristicsAsync();
                    foreach (var c in characteristics) {
                        if (c.Uuid == INPUT_CHARACTERISTIC_UUID) {
                            Debug.WriteLine("Found input forwarding characteristic");
                            SetInput = c;
                        }
                    }
                } else if (service.Uuid == GUNDAM_SERVICE_UUID) {
                    Debug.WriteLine("Found Gundam service");
                    var characteristics = await service.GetCharacteristicsAsync();
                    foreach (var c in characteristics) {
                        if (c.Uuid == GUNDAM_CHARACTERISTIC_CHANGE_CAMERA_UUID) {
                            Debug.WriteLine("Found Gundam camera change characteristic");
                            GundamCameraChange = c;
                        } else if (c.Uuid == GUNDAM_CHARACTERISTIC_RESET_CAMERA_UUID) {
                            Debug.WriteLine("Found Gundam camera reset characteristic");
                            GundamCameraReset = c;
                        }
                    }
                }
            }


            return true;
        }

        public void Disconnect() {
            GundamCameraChange = null;
            GundamCameraReset = null;
            SetInput = null;
            Handle.CancelConnection();
        }

        public IGattCharacteristic GundamCameraChange;
        public IGattCharacteristic GundamCameraReset;
        public IGattCharacteristic SetInput;

        public static DeviceModel ActiveDevice;

        public static String INPUT_SERVICE_UUID = "1209214d-246d-2815-27c6-f57dad450100";
        public static String INPUT_CHARACTERISTIC_UUID = "1209214d-246d-2815-27c6-f57dad450101";

        public static String GUNDAM_SERVICE_UUID = "1209214d-246d-2815-27c6-f57dad452800";
        public static String GUNDAM_CHARACTERISTIC_CHANGE_CAMERA_UUID = "1209214d-246d-2815-27c6-f57dad452801";
        public static String GUNDAM_CHARACTERISTIC_RESET_CAMERA_UUID = "1209214d-246d-2815-27c6-f57dad452802";
    }
}