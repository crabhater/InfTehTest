using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.DataContext
{
    public class FileApiRepository : IRepository<FolderFileViewModel>
    {
        private IApiService _apiService;
        public FileApiRepository(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task CreateAsync(FolderFileViewModel item)
        {
            await _apiService.AddFileAsync(item);
        }

        public async Task DeleteAsync(FolderFileViewModel item)
        {
            await _apiService.DeleteFileAsync(item.Id);
        }


        public async Task<FolderFileViewModel> GetAsync(FolderFileViewModel item)
        {
            return await _apiService.GetFileContentAsync(item.Id);
        }

        public Task<ObservableCollection<FolderFileViewModel>> GetList()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(FolderFileViewModel item)
        {
            await _apiService.UpdateFileAsync(item);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _apiService.Dispose();
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
