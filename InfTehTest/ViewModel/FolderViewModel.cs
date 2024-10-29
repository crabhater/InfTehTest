using InfTehTest.Command;
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
using System.Windows;
using System.Windows.Input;

namespace InfTehTest.ViewModel
{
    public class FolderViewModel : IBaseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private ObservableCollection<IBaseVM> _child;
        public ObservableCollection<IBaseVM> Child { get { return _child; } set { _child = value; OnPropertyChanged(); } }
        //public ObservableCollection<FolderFileViewModel> Files { get { return _files; } set { _files = value; OnPropertyChanged(); } }
        public string Icon { get; set; }
        public int? FolderId { get; set; }

        private Visibility _textBlockVisibility = Visibility.Visible;
        private Visibility _textBoxVisibility = Visibility.Collapsed;

        public Visibility TextBlockVisibility
        {
            get
            {
                return _textBlockVisibility;
            }
            set
            {
                _textBlockVisibility = value; OnPropertyChanged();
            }
        }
        public Visibility TextBoxVisibility
        {
            get
            {
                return _textBoxVisibility;
            }
            set
            {
                _textBoxVisibility = value; OnPropertyChanged();
            }
        }

        public void EnableEdit()
        {
            TextBlockVisibility = Visibility.Collapsed;
            TextBoxVisibility = Visibility.Visible;
        }
        public void DisableEdit()
        {
            TextBoxVisibility = Visibility.Collapsed;
            TextBlockVisibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
