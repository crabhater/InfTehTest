using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InfTehTest.InterfacesLib
{
    public interface IBaseVM : INotifyPropertyChanged
    {
        int Id { get; set; }
        string Name { get; set; }
        int? FolderId { get; set; }
        string Icon { get; set; }
        event PropertyChangedEventHandler? PropertyChanged;
        void EnableEdit();
        void DisableEdit();
    }
}
