using System.ComponentModel;
using Xamarin.Forms;
using XamarinWorkshop.ViewModels;

namespace XamarinWorkshop.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}