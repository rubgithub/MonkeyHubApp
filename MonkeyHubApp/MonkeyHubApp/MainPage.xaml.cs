using MonkeyHubApp.Model;
using MonkeyHubApp.Services;
using MonkeyHubApp.ViewModels;
using Xamarin.Forms;

namespace MonkeyHubApp
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel => BindingContext as MainViewModel;
        public MainPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IMonkeyHubApiService>();
            BindingContext = new MainViewModel(service);
            //BindingContext = new MainViewModel(new MonkeyHubApiService());
        }

        //private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var tag = (sender as ListView)?.SelectedItem as Tag;
        //    (BindingContext as MainViewModel)?.ShowCategoryCommand.Execute(tag);
        //}
    }
}
