using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.ViewModel
{
    public class FolderViewModel
    {
        public int Id { get; set; }
        public string FolderName { get; set; }
        public ObservableCollection<FolderFileViewModel> Files { get; set; }
        public ObservableCollection<FolderViewModel> Folders { get; set; }
        public int? ParentFolderId { get; set; }
        public string Icon { get { return "\\folder.png"; } }
    }
}
