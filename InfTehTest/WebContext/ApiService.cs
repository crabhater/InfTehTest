using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task<List<FolderViewModel>> GetFoldersAsync(int folderId)
        {
            //var response = await _httpClient.GetStringAsync($"api/files/folders?parentFolderId={parentFolderId}");
            var endPoint = $"api/Folder/GetFolderContent/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            return JsonConvert.DeserializeObject<List<FolderViewModel>>(response);
        }

        public async Task<List<FolderFileViewModel>> GetFilesAsync(int folderId)
        {
            var endPoint =  $"api/folder/GetFolderContent/{folderId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            return JsonConvert.DeserializeObject<List<FolderFileViewModel>>(response);
        }

        public async Task<FolderFileViewModel> GetFileContentAsync(int fileId)
        {
            var endPoint = $"api/Folder/GetFileContent/{fileId}";
            var response = await SendRequestAsync(HttpMethod.Get, endPoint, null, null);
            return JsonConvert.DeserializeObject<FolderFileViewModel>(response);
        }


        public async Task<string> SendRequestAsync(HttpMethod method, string url, string content, Dictionary<string, string> headers)
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
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                };
            }

            //client.Timeout = TimeSpan.FromMinutes(10); //Увеличение таймаута соединения для 

            using (var response = await _httpClient.SendAsync(request))
            {
                //response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();
            }


            return body;
        }
    }
}
