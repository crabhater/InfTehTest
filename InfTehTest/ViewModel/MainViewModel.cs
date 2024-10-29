using InfTehTest.Command;
using InfTehTest.DataContext;
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
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InfTehTest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;
        private readonly IRepository<FolderViewModel> _folderRepository;
        private readonly IRepository<FolderFileViewModel> _fileRepository;
        private ObservableCollection<IBaseVM> _folders;
        public ObservableCollection<IBaseVM> Folders { get { return _folders; } set{ _folders = value; OnPropertyChanged(); } }
        public ObservableCollection<IBaseVM> OpenTabs { get; set; }
        private IBaseVM _selectedItem;
        public IBaseVM SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            var httpClient = new HttpClient();
            var apiService = new ApiService(httpClient);
            _apiService = apiService;
            _folderRepository = new FolderApiRepository(apiService);
            _fileRepository = new FileApiRepository(apiService);
            OpenTabs = new ObservableCollection<IBaseVM>();
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
                        if(SelectedItem == null || SelectedItem.GetType() != typeof(FolderViewModel))
                        {
                            MessageBox.Show("Выбери папку куда будем сохранять");
                            return;
                        }

                        var newFolder = new FolderViewModel()
                        {
                            FolderId = SelectedItem.Id,
                            Name = "Новая папка",
                            Icon = "",
                            Child = new ObservableCollection<IBaseVM>(),
                        };
                        
                        
                        await _folderRepository.CreateAsync(newFolder);
                        //await _folderRepository.GetAsync((FolderViewModel)SelectedItem);
                        await Folders.FindAndDoActionAsync(SelectedItem, async e =>
                        {
                            OpenFolder((FolderViewModel)e);
                        });
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
                        if (SelectedItem == null || SelectedItem.GetType() != typeof(FolderViewModel))
                        {
                            MessageBox.Show("Выбери папку для удаления");
                        }

                        var curFolder = new FolderViewModel()
                        {
                            Id = (int)SelectedItem.FolderId,
                        };
                        await _folderRepository.DeleteAsync((FolderViewModel)SelectedItem);
                        await Folders.FindAndDoActionAsync(curFolder, async e =>
                        {
                            OpenFolder((FolderViewModel)e);
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
                        if (SelectedItem == null && SelectedItem.GetType() != typeof(FolderFileViewModel))
                        {
                            MessageBox.Show("Выбери файл для удаления");
                        }

                        var curFolder = new FolderViewModel
                        {
                            Id = (int)SelectedItem.FolderId,
                        };

                        await _fileRepository.DeleteAsync((FolderFileViewModel)SelectedItem);
                        await Folders.FindAndDoActionAsync(curFolder, async e =>
                        {
                            OpenFolder((FolderViewModel)e);
                        });
                        OpenTabs.Remove(SelectedItem);
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
                        if (SelectedItem == null || SelectedItem.GetType() != typeof(FolderViewModel))
                        {
                            MessageBox.Show("Выбери папку куда будем наш файл грузить");
                            return;
                        }
                        
                        var openFileDialog = new OpenFileDialog();
                        var folderName = string.Empty;
                        if (openFileDialog.ShowDialog() == true)
                        {
                            folderName = openFileDialog.FileName;
                        }
                        using StreamReader sr = new StreamReader(folderName);
                        var content = sr.ReadToEnd();
                        var name = Path.GetFileNameWithoutExtension(folderName);
                        var fileType = Path.GetExtension(folderName).Trim('.');
                        var newFile = new FolderFileViewModel
                        {
                            Content = content,
                            Name = name,
                            FolderId = SelectedItem.Id,
                            Icon = "",
                            Description = "",
                            FileTypeName = fileType
                        };

                        await _fileRepository.CreateAsync(newFile);
                        await Folders.FindAndDoActionAsync((FolderViewModel)SelectedItem, async e =>
                        {
                            OpenFolder((FolderViewModel)e);
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
                    (_downloadFileCommand = new RelayCommand(async () =>
                    {
                        if (_selectedItem == null || _selectedItem.GetType() != typeof(FolderFileViewModel))
                        {
                            MessageBox.Show("Выбери файл для скачивания");
                            return;
                        }
                        var openFolderDialog = new OpenFolderDialog();
                        var folderName = string.Empty;
                        if (openFolderDialog.ShowDialog() == true)
                        {
                            folderName = openFolderDialog.FolderName;
                        }
                        folderName += "\\" + (SelectedItem as FolderFileViewModel).FullName;
                        using StreamWriter sw = new StreamWriter(folderName);
                        await _fileRepository.GetAsync((FolderFileViewModel)SelectedItem);
                        sw.WriteLine((SelectedItem as FolderFileViewModel).Content);
                        MessageBox.Show($"Файл сохранен в {folderName}");
                    }));
            }
        }
        private RelayCommand _startEditCommand;
        public RelayCommand StartEditCommand
        {
            get
            {
                return _startEditCommand ??
                    (_startEditCommand = new RelayCommand(async () =>
                    {
                        if (SelectedItem == null)
                        {
                            MessageBox.Show("Выбери файл или папку");
                            return;
                        }
                        SelectedItem.EnableEdit();
                    }
                    ));
            }
        }

        public async Task EndEdit(string newName)
        {
            SelectedItem.DisableEdit();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Недопустимое имя");
                await DropEdit();
                return;
            }
            int folderId = 0;

            if(SelectedItem.GetType() == typeof(FolderFileViewModel))
            {
                await _fileRepository.GetAsync((FolderFileViewModel)SelectedItem);
                SelectedItem.Name = newName;
                await _fileRepository.UpdateAsync((FolderFileViewModel)SelectedItem);
                folderId = (int)SelectedItem.FolderId;
            }

            if(SelectedItem.GetType() == typeof(FolderViewModel))
            {
                await _folderRepository.GetAsync((FolderViewModel)SelectedItem);
                SelectedItem.Name = newName;
                await _folderRepository.UpdateAsync((FolderViewModel)SelectedItem);
                folderId = SelectedItem.Id;
            }


            
            var curFolder = new FolderViewModel
            {
                Id = (int)SelectedItem.FolderId,
            };
            await Folders.FindAndDoActionAsync(curFolder, async e =>
            {
                OpenFolder((FolderViewModel)e);
            });
            OnPropertyChanged();
        }

        public async Task DropEdit()
        {
            SelectedItem.DisableEdit();
            
            var curFolder = new FolderViewModel
            {
                Id = SelectedItem.Id,
            };
            await Folders.FindAndDoActionAsync(curFolder, async e =>
            {
                OpenFolder((FolderViewModel)e);
            });
        }

        public async void LoadData()
        {
            var stub = new FolderViewModel()
            {
                Id = 1
            };
            var content = await _folderRepository.GetAsync(stub);
            Folders = content.Child;
        }

        public async void OpenFolder()
        {
            if(SelectedItem.GetType() == typeof(FolderViewModel))
            {
                await Folders.FindAndDoActionAsync((FolderViewModel)SelectedItem, async e =>
                {
                    OpenFolder((FolderViewModel)e);
                });
            }
            if(SelectedItem.GetType() == typeof(FolderFileViewModel))
            {
                var curFolder = new FolderViewModel()
                {
                    Id = (int)SelectedItem.FolderId
                };
                await Folders.FindAndDoActionAsync(SelectedItem, async e =>
                {
                    OpenFile((FolderFileViewModel)e);
                });
            }

        }

        private async void OpenFolder(FolderViewModel item)
        {
            var folderChild = await _folderRepository.GetAsync(item);
            item.Child = folderChild.Child;
            //tvvm.ad = folderChild.Files;
            OnPropertyChanged();
        }

        private async void OpenFile(FolderFileViewModel item)
        {
            var file = await _fileRepository.GetAsync(item);
            item.Content = file.Content;
            if (!OpenTabs.Contains(item))
            {
                OpenTabs.Add(item);
            }
            SelectedItem = item;
            OnPropertyChanged();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
