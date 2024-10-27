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
using System.Windows.Input;

namespace InfTehTest.ViewModel
{
    public class FolderViewModel : TreeViewVM, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get ; set ; }
        public ObservableCollection<TreeViewVM> Child { get ; set ; }
        public string Icon { get ; set ; }
        public int? ParentFolderId { get; set; }
        public string Content { get => null; set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
