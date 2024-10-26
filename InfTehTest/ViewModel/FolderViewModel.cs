using InfTehTest.Command;
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
    public class FolderViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<FolderFileViewModel> Files { get; set; }
        public ObservableCollection<FolderViewModel> Folders { get; set; }
        public int? ParentFolderId { get; set; }
        public string Icon { get { return "\\folder.png"; } }

        public bool HasDummyChild { get; set; }
        public ICommand LoadSubFoldersCommand { get; set; }

        public FolderViewModel()
        {
            Files = new ObservableCollection<FolderFileViewModel>();
            Folders = new ObservableCollection<FolderViewModel>();
            HasDummyChild = true;
            LoadSubFoldersCommand = new RelayCommand(() => LoadSubFolders());
        }

        private async void LoadSubFolders()
        {
            if (!HasDummyChild)
                return;

            HasDummyChild = false;
            var apiService = new ApiService(new HttpClient { BaseAddress = new Uri("https://localhost:7185/") });
            var subFolders = await apiService.GetFoldersAsync(Id);
            Folders.Clear();
            foreach (var folder in subFolders)
            {
                Folders.Add(folder);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
