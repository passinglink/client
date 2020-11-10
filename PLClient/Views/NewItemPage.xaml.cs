using PLClient.Models;
using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class NewItemPage : ContentPage {
        public Item Item { get; set; }

        public NewItemPage() {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}