using Xamarin.Forms;

namespace MedicalTasks
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
    }
}
