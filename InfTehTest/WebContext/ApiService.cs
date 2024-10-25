using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.WebContext
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FolderViewModel>> GetFoldersAsync(int parentFolderId)
        {
            var response = await _httpClient.GetStringAsync($"api/files/folders?parentFolderId={parentFolderId}");
            return JsonConvert.DeserializeObject<List<FolderViewModel>>(response);
        }

        public async Task<List<FolderFileViewModel>> GetFilesAsync(int folderId)
        {
            var response = await _httpClient.GetStringAsync($"api/files/files/{folderId}");
            return JsonConvert.DeserializeObject<List<FolderFileViewModel>>(response);
        }

        public async Task<FolderFileViewModel> GetFileContentAsync(int fileId)
        {
            var response = await _httpClient.GetStringAsync($"api/files/file/{fileId}");
            return JsonConvert.DeserializeObject<FolderFileViewModel>(response);
        }
    }
}
