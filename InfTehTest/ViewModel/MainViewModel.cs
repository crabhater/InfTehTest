using InfTehTest.InterfacesLib;
using InfTehTest.WebContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InfTehTest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;
        private ObservableCollection<FolderViewModel> _folders;
        public ObservableCollection<FolderViewModel> Folders { get { return _folders; } set { _folders = value; OnPropertyChanged(); } }
        public ObservableCollection<FolderFileViewModel> OpenTabs { get; set; }

        private FolderFileViewModel _selectedFile;
        public FolderFileViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7185/") };
            _apiService = new ApiService(httpClient);
            LoadDataAsync();
            OpenTabs = new ObservableCollection<FolderFileViewModel>();
        }
        private async Task LoadDataAsync()
        {
            var folders = await _apiService.GetFoldersAsync(2); //TODO проброс id из вьюшки 
            Folders = new ObservableCollection<FolderViewModel>(folders);
        }

        public async void OpenFolder(FolderViewModel folder)
        {
            if (folder != null)
            {
                var folders = await _apiService.GetFoldersAsync(folder.Id);
                Folders = new ObservableCollection<FolderViewModel>(folders);
            }
        } 

        public async void OpenFile(FolderFileViewModel file)
        {
            if (file != null && !OpenTabs.Contains(file))
            {
                var fileContent = await _apiService.GetFileContentAsync(file.Id);
                file.Content = fileContent.Content;
                OpenTabs.Add(file);
            }
            SelectedFile = file;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
