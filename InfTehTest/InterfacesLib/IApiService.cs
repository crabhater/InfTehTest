using InfTehTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.InterfacesLib
{
    public interface IApiService
    {
        Task<List<FolderViewModel>> GetFoldersAsync(int parentFolderId);
        Task<List<FolderFileViewModel>> GetFilesAsync(int folderId);
        Task<FolderFileViewModel> GetFileContentAsync(int fileId);
        Task<FolderViewModel> GetFolderContentAsync(int folderId);

    }
}
