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
    public class InputForwardingViewModel : BaseViewModel {
        IGamepadService gamepad_svc;

        IGamepad _gamepad;
        IGamepad Gamepad {
            get => _gamepad;
            set {
                SetProperty(ref _gamepad, value);
                OnPropertyChanged("ControllerName");
            }
        }

        // TODO: There has to be a better way to do this...
        bool _forwarding = false;
        public bool Forwarding {
            get => _forwarding;
            set {
                _forwarding = value;
                OnPropertyChanged("Forwarding");
                OnPropertyChanged("ForwardButtonText");
            }
        }

        public string ControllerName {
            get => _gamepad != null ? _gamepad.Name() : "None";
        }

        public string ForwardButtonText {
            get => Forwarding ? "Stop" : "Start";
        }

        Thread forwardThread;
        CancellationTokenSource forwardThreadCancel;

        public Command ToggleForwardingCommand { get; }

        public InputForwardingViewModel() {
            gamepad_svc = DependencyService.Get<IGamepadService>();
            gamepad_svc.Gamepads.CollectionChanged += (sender, e) => {
                lock (gamepad_svc) {
                    if (!gamepad_svc.Gamepads.Contains(Gamepad)) {
                        Gamepad = null;
                        StopForwarding();
                    }

                    if (!gamepad_svc.Gamepads.IsEmpty()) {
                        Gamepad = gamepad_svc.Gamepads[0];
                    }
                }
            };
            Title = "Input Forwarding";

            ToggleForwardingCommand = new Command(async () => await ExecuteToggleForwardingCommand());
        }

        void StartForwarding() {
            if (Gamepad != null) {
                forwardThreadCancel = new CancellationTokenSource();
                forwardThread = new Thread(() => {
                    CancellationToken token = forwardThreadCancel.Token;
                    while (!token.IsCancellationRequested && Gamepad != null) {
                        byte[] inputs = Gamepad.GetInputData();
                        if (inputs != null) {
                            DeviceModel.ActiveDevice.SetInput.Write(inputs);
                        }
                        Thread.Sleep(15);
                    }
                });
                Forwarding = true;
            } else {
                Debug.WriteLine("Not starting, no gamepad available");
            }
        }

        void StopForwarding() {
            if (forwardThreadCancel != null) {
                forwardThreadCancel.Cancel();
                forwardThreadCancel = null;
                forwardThread = null;
            }
            Forwarding = false;
        }


        async Task ExecuteToggleForwardingCommand() {
            Debug.WriteLine($"Toggling forwarding");
            if (!Forwarding) {
                StopForwarding();
            } else {
                StartForwarding();
            }
        }

        public void OnAppearing() {
        }
    }
}
