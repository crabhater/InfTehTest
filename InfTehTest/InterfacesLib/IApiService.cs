using InfTehTest.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.InterfacesLib
{
    public interface IApiService : IDisposable
    {
        Task<List<FolderViewModel>> GetFoldersAsync(int parentFolderId);
        Task<List<FolderFileViewModel>> GetFilesAsync(int folderId);
        Task<TreeViewVM> GetFileContentAsync(int fileId);
        Task<TreeViewVM> GetFolderContentAsync(int folderId);
        Task DeleteFolderAsync(int folderId);
        Task DeleteFileAsync(int fileId);
        Task<TreeViewVM> AddFolderAsync(TreeViewVM folder);
        Task<TreeViewVM> AddFileAsync(TreeViewVM file);
        Task UpdateFolderAsync(TreeViewVM folder);
        Task UpdateFileAsync(TreeViewVM file);

    }
}
