using PLClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace PLClient.UWP {
    class PLGamepadService : IGamepadService {
        ObservableCollection<IGamepad> _gamepads = new ObservableCollection<IGamepad>();
        public ObservableCollection<IGamepad> Gamepads {
            get => _gamepads;
        }

        public PLGamepadService() {
            foreach (var gamepad in Windows.Gaming.Input.Gamepad.Gamepads) {
                _gamepads.Add(new PLGamepad(gamepad));
            }
            Gamepad.GamepadAdded += (sender, gamepad) => {
                lock (this) {
                    _gamepads.Add(new PLGamepad(gamepad));
                }
            };
        }
    }

    class PLGamepad : IGamepad {
        Gamepad _gamepad;

        public PLGamepad(Gamepad g) {
            Debug.WriteLine("Gamepad added");
            _gamepad = g;
        }

        public byte[] GetInputData() {
            BitVector32 bv = new BitVector32();
            var input = _gamepad.GetCurrentReading();
            int i = 0;
            bv[i++] = (input.Buttons & GamepadButtons.Y) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.B) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.A) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.X) != GamepadButtons.None;

            bv[i++] = (input.Buttons & GamepadButtons.LeftShoulder) != GamepadButtons.None;
            bv[i++] = input.LeftTrigger > 100;
            bv[i++] = (input.Buttons & GamepadButtons.LeftThumbstick) != GamepadButtons.None;

            bv[i++] = (input.Buttons & GamepadButtons.RightShoulder) != GamepadButtons.None;
            bv[i++] = input.RightTrigger > 100;
            bv[i++] = (input.Buttons & GamepadButtons.RightThumbstick) != GamepadButtons.None;

            bv[i++] = false;
            bv[i++] = (input.Buttons & GamepadButtons.Menu) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.View) != GamepadButtons.None;
            bv[i++] = false;

            bv[i++] = (input.Buttons & GamepadButtons.DPadUp) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.DPadDown) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.DPadRight) != GamepadButtons.None;
            bv[i++] = (input.Buttons & GamepadButtons.DPadLeft) != GamepadButtons.None;

            bv[i++] = false;
            bv[i++] = false;
            bv[i++] = false;

            var array = BitConverter.GetBytes(bv.Data);
            return new byte[] { array[0], array[1], array[2] };
        }

        public string Name() {
            return "Gamepad";
        }
    }
}
