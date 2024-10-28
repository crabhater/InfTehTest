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
using System.Windows.Controls;

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
            var endPoint = $"api/Folder/DeleteFolder/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Delete, endPoint, null, null);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var endPoint = $"api/Folder/DeleteFile/{fileId}";
            var response = await SendRequestAsync(HttpMethod.Delete, endPoint, null, null);
        }


        public async Task<TreeViewVM> AddFolderAsync(TreeViewVM folder)
        {
            var endPoint = $"api/Folder/AddFolder";
            var content = JsonConvert.SerializeObject(folder);
            var response = await SendRequestAsync(HttpMethod.Post, endPoint, content, null);
            return JsonConvert.DeserializeObject<TreeViewVM>(response);
        }

        public async Task UpdateFolderAsync(TreeViewVM folder)
        {
            var endPoint = $"api/Folder/UpdateFolder/{folder.Id}";
            var content = JsonConvert.SerializeObject(folder);
            var response = await SendRequestAsync(HttpMethod.Put, endPoint, content, null);
        }

        public async Task UpdateFileAsync(TreeViewVM file)
        {
            var endPoint = $"api/Folder/UpdateFile/{file.Id}";
            var content = JsonConvert.SerializeObject(file);
            var response = await SendRequestAsync(HttpMethod.Put, endPoint, content, null);
        }

        public async Task<TreeViewVM> AddFileAsync(TreeViewVM file)
        {
            var endPoint = $"api/Folder/AddFile";
            var content = JsonConvert.SerializeObject(file);
            var response = await SendRequestAsync(HttpMethod.Post, endPoint, content, null);
            return JsonConvert.DeserializeObject<TreeViewVM>(response);
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

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                request.Content = new StringContent(content)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                };
            }

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
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
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
