using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
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

        public async Task<TreeViewVM> GetFolderContentAsync(int folderId)
        {
            var endPoint = $"api/Folder/GetFolderContent/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            return JsonConvert.DeserializeObject<TreeViewVM>(response);
        }

        public async Task<TreeViewVM> GetFileContentAsync(int fileId)
        {
            var endPoint = $"api/Folder/GetFileContent/{fileId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            return JsonConvert.DeserializeObject<TreeViewVM>(response);
        }

        public async Task DeleteFolderAsync(int folderId)
        {
            var endPoint = $"/api/Folder/DeleteFolder/folder/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Delete, endPoint, null, null);
        }


        public async Task AddFolderAsync(TreeViewVM folder)
        {
            var endPoint = $"/api/Folder/AddFolder/folder{folder}";
            var response = await SendRequestAsync(HttpMethod.Delete, endPoint, null, null);
        }

        public async Task UpdateFolderAsync(TreeViewVM folder)
        {
            var endPoint = $"/api/Folder/AddFolder";
            var content = JsonConvert.SerializeObject(folder);
            var response = await SendRequestAsync(HttpMethod.Delete, endPoint, content, null);
        }

        public async Task AddFileAsync(TreeViewVM file)
        {
            var endPoint = $"/api/Folder/AddFile";
            var content = JsonConvert.SerializeObject(file);
            var response = await SendRequestAsync(HttpMethod.Post, endPoint, content, null);
        }


        private async Task<string> SendRequestAsync(HttpMethod method, string url, string content, Dictionary<string, string> headers)
        {
            string body = "";
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new UriBuilder($"https://localhost:7185/{url}").Uri,
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (method == HttpMethod.Post)
            {
                request.Content = new StringContent(content)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                };
            }

            using (var response = await _httpClient.SendAsync(request))
            {
                body = await response.Content.ReadAsStringAsync();
            }

            return body;
        }

        public Task<List<FolderViewModel>> GetFoldersAsync(int parentFolderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FolderFileViewModel>> GetFilesAsync(int folderId)
        {
            throw new NotImplementedException();
        }
    }

}
