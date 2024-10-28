using InfTehTest.Command;
using InfTehTest.Extensions;
using InfTehTest.InterfacesLib;
using InfTehTest.WebContext;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InfTehTest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;
        private ObservableCollection<TreeViewVM> _folders;
        public ObservableCollection<TreeViewVM> Folders { get { return _folders; } set{ _folders = value; OnPropertyChanged(); } }
        public ObservableCollection<TreeViewVM> OpenTabs { get; set; }

        private TreeViewVM _selectedFile;
        public TreeViewVM SelectedFile
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
            var httpClient = new HttpClient();
            _apiService = new ApiService(httpClient);
            OpenTabs = new ObservableCollection<TreeViewVM>();
            LoadData();

        }
        private RelayCommand _createFolderCommand;
        public RelayCommand CreateFolderCommand 
        { 
            get
            {
                return _createFolderCommand ??
                    (_createFolderCommand = new RelayCommand(async () =>
                    {
                        int folderId;
                        switch (SelectedFile.TypeId)
                        {
                            case 0: folderId = SelectedFile.Id; break;
                            default: folderId = SelectedFile.FolderId; break;
                        }

                        var curFolder = new TreeViewVM
                        {
                            Id = folderId,
                            TypeId = 0
                        };

                        var newFolder = new TreeViewVM
                        {
                            FolderId = folderId,
                            Name = "Новая папка",
                            TypeId = 0,
                            Icon = "\\folder.png"
                        };
                        await _apiService.AddFolderAsync(newFolder);

                        await Folders.FindAndDoActionAsync(curFolder, (e =>
                        {
                            OpenFolder(e);
                        }));
                        
                    }));
            }
        }
        private RelayCommand _removeFolderCommand;
        public RelayCommand RemoveFolderCommand
        {
            get
            {
                return _removeFolderCommand ??
                    (_removeFolderCommand = new RelayCommand(async () =>
                    {
                        int folderId;
                        switch (SelectedFile.TypeId)
                        {
                            case 0: folderId = SelectedFile.Id; break;
                            default: folderId = SelectedFile.FolderId; break;
                        }

                        var curFolder = new TreeViewVM
                        {
                            Id = folderId,
                            TypeId = 0
                        };

                        await _apiService.DeleteFolderAsync(folderId);
                        await Folders.FindAndDoActionAsync(curFolder, e =>
                        {
                            OpenFolder(e);
                        });
                    }));
            }
        }

        private RelayCommand _removeFileCommand;
        public RelayCommand RemoveFileCommand
        {
            get
            {
                return _removeFileCommand ??
                    (_removeFileCommand = new RelayCommand(async () =>
                    {
                        int folderId;
                        switch (SelectedFile.TypeId)
                        {
                            case 0: return;
                            default: folderId = SelectedFile.FolderId; break;
                        }

                        var curFolder = new TreeViewVM
                        {
                            Id = folderId,
                            TypeId = 0
                        };

                        await _apiService.DeleteFileAsync(SelectedFile.Id);
                        await Folders.FindAndDoActionAsync(curFolder, e =>
                        {
                            OpenFolder(e);
                        });
                    }));
            }
        }

        private RelayCommand _uploadFileCommand;
        public RelayCommand UploadFileCommand
        {
            get
            {
                return _uploadFileCommand ??
                    (_uploadFileCommand = new RelayCommand(async () =>
                    {
                        int folderId;
                        switch (SelectedFile.TypeId)
                        {
                            case 0: folderId = SelectedFile.Id; break;
                            default: folderId = SelectedFile.FolderId; break;
                        }
                        var openFileDialog = new OpenFileDialog();
                        var folderName = string.Empty;
                        if (openFileDialog.ShowDialog() == true)
                        {
                            folderName = openFileDialog.FileName;
                        }
                        using StreamReader sr = new StreamReader(folderName);
                        var content = sr.ReadToEnd();
                        var name = Path.GetFileNameWithoutExtension(content);
                        var newFile = new TreeViewVM
                        {
                            Content = content,
                            Name = name,
                            FolderId = folderId,
                            TypeId = 1 //TODO Заменить
                        };

                        var curFolder = new TreeViewVM
                        {
                            Id = folderId,
                            TypeId = 0
                        };

                        await _apiService.AddFileAsync(newFile);
                        await Folders.FindAndDoActionAsync(curFolder, e => 
                        {
                            OpenFolder(e);
                        });

                    }));
            }
        }
        private RelayCommand _downloadFileCommand;
        public RelayCommand DownloadFileCommand
        {
            get
            {
                return _downloadFileCommand ??
                    (_downloadFileCommand = new RelayCommand(async ()=>
                    {
                        var openFolderDialog = new OpenFolderDialog();
                        var folderName = string.Empty;
                        if (openFolderDialog.ShowDialog() == true)
                        {
                            folderName = openFolderDialog.FolderName;
                        }
                        folderName += "\\" + SelectedFile.Name + ".txt";
                        using StreamWriter sw = new StreamWriter(folderName);
                        sw.WriteLine(SelectedFile.Content);
                        MessageBox.Show($"Файл сохранен в {folderName}");
                    }));
            }
        }
        private RelayCommand _renameCommand;
        public RelayCommand RenameCommand
        {
            get
            {
                return _renameCommand ??
                    (_renameCommand = new RelayCommand(async () =>
                    {
                        var newName = "Новое имя";
                        SelectedFile.Name = newName;
                        await _apiService.UpdateFileAsync(SelectedFile);
                        int folderId;

                        switch (SelectedFile.TypeId)
                        {
                            case 0: folderId = SelectedFile.Id; break;
                            default: folderId = SelectedFile.FolderId; break;
                        }
                        var curFolder = new TreeViewVM
                        {
                            Id = folderId,
                            TypeId = 0
                        };
                        await Folders.FindAndDoActionAsync(curFolder, e =>
                        {
                            OpenFolder(e);
                        });
                    }
                    ));
            }
        }

        public async void LoadData()
        {
            var folder = await _apiService.GetFolderContentAsync(1);
            Folders = folder.Child;
        }

        public async void OpenFolder(TreeViewVM tvvm)
        {
            if(tvvm != null)
            {
                switch (tvvm.TypeId)
                {
                    case 0:
                        var folderChild = await _apiService.GetFolderContentAsync(tvvm.Id);
                        tvvm.Child = folderChild.Child; break;
                    default:
                        var fileContent = await _apiService.GetFileContentAsync(tvvm.Id);
                        tvvm.Content = fileContent.Content;
                        OpenTabs.Add(tvvm); 
                        break;
                }
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
