using PLClient.ViewModels;
using Xamarin.Forms;

namespace PLClient.Views {
    public partial class ItemDetailPage : ContentPage {
        public ItemDetailPage() {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}