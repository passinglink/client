using System;
using Android.App;
using Android.Runtime;

[Application]
public class MainApplication : Application {
    public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {
    }


    public override void OnCreate() {
        base.OnCreate();
    }
}
