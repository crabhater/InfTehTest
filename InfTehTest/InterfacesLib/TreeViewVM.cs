using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.InterfacesLib
{
    public class TreeViewVM : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private ObservableCollection<TreeViewVM> _child;
        public ObservableCollection<TreeViewVM> Child { get { return _child; } set { _child = value; OnPropertyChanged(); } }
        public int TypeId { get; set; }
        public int FolderId { get; set; }
        public string Icon { get; set; }
        private string _content;
        public string Content { get { return _content; } set{ _content = value; OnPropertyChanged(); } }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
