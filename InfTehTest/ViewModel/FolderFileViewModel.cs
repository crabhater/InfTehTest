using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.ViewModel
{
    public class FolderFileViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string? Description { get; set; }
        public int? FileTypeId { get; set; }
        public string FileTypeName { get; set; }
        public string? Icon { get; set; }
        public int FolderId { get; set; }
        public string? Content { get; set; }
    }
}
