using InfTehTest.InterfacesLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InfTehTest.ViewModel
{
    public class FolderFileViewModel : IBaseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FileTypeName { get; set; }
        public string? Icon { get; set; }
        public int? FolderId { get; set; }
        public string? Content { get; set; }
        public string FullName { get
            {
                return $"{Name}.{FileTypeName}";
            } }

        private Visibility _textBlockVisibility = Visibility.Visible;
        private Visibility _textBoxVisibility = Visibility.Collapsed;

        public Visibility TextBlockVisibility { get 
            {
                return _textBlockVisibility;
            } set 
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
            TextBoxVisibility= Visibility.Collapsed;
            TextBlockVisibility= Visibility.Visible;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
