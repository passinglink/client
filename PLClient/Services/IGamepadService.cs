using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PLClient.Services {
    public interface IGamepadService {
        ObservableCollection<IGamepad> Gamepads { get; }
    }

    public interface IGamepad {
        string Name();
        byte[] GetInputData();
    }
}
