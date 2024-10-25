using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using InfTehTest.WebContext;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace InfTehTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IApiService _apiService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7185/") };
            _apiService = new ApiService(httpClient);

            var mainWindow = new MainWindow();
            var mainViewModel = new MainViewModel(_apiService);
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();

        }

    }
}
