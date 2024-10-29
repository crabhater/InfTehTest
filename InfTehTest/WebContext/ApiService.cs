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

        public async Task<List<IBaseVM>> GetFolderFilesAsync(int folderId)
        {
            var endPoint = $"api/Folder/GetFolderFiles/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            var result = JsonConvert.DeserializeObject<List<FolderFileViewModel>>(response);
            var list = new List<IBaseVM>();
            foreach (var res in result)
            {
                list.Add(res);
            }
            return list;
        }

        public async Task<List<IBaseVM>>GetFolderFoldersAsync(int folderId)
        {
            var endPoint = $"api/Folder/GetFolderFolders/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            var result = JsonConvert.DeserializeObject<List<FolderViewModel>>(response);
            var list = new List<IBaseVM>();
            foreach(var res in result)
            {
                list.Add(res);
            }
            return list;
        }

        public async Task<FolderFileViewModel> GetFileContentAsync(int fileId)
        {
            var endPoint = $"api/Folder/GetFileContent/{fileId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            return JsonConvert.DeserializeObject<FolderFileViewModel>(response);
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


        public async Task AddFolderAsync(FolderViewModel folder)
        {
            var endPoint = $"api/Folder/AddFolder";
            var content = JsonConvert.SerializeObject(folder);
            var response = await SendRequestAsync(HttpMethod.Post, endPoint, content, null);
        }

        public async Task UpdateFolderAsync(FolderViewModel folder)
        {
            var endPoint = $"api/Folder/UpdateFolder";
            var content = JsonConvert.SerializeObject(folder);
            var response = await SendRequestAsync(HttpMethod.Put, endPoint, content, null);
        }

        public async Task UpdateFileAsync(FolderFileViewModel file)
        {
            var endPoint = $"api/Folder/UpdateFile";
            var content = JsonConvert.SerializeObject(file);
            var response = await SendRequestAsync(HttpMethod.Put, endPoint, content, null);
        }

        public async Task AddFileAsync(FolderFileViewModel file)
        {
            var endPoint = $"api/Folder/AddFile";
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
