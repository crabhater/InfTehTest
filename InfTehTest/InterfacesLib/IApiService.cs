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
        Task<FolderFileViewModel> GetFileContentAsync(int fileId);
        Task<List<IBaseVM>> GetFolderFilesAsync(int folderId);
        Task<List<IBaseVM>> GetFolderFoldersAsync(int folderId);
        Task DeleteFolderAsync(int folderId);
        Task DeleteFileAsync(int fileId);
        Task AddFolderAsync(FolderViewModel folder);
        Task AddFileAsync(FolderFileViewModel file);
        Task UpdateFolderAsync(FolderViewModel folder);
        Task UpdateFileAsync(FolderFileViewModel file);

    }
}
