using PLClient.Services;
using Xamarin.Forms;

namespace PLClient {
    public partial class App : Application {

        public App() {
            InitializeComponent();

            App.Current.UserAppTheme = OSAppTheme.Light;
            MainPage = new AppShell();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
