using InfTehTest.Extensions;
using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.DataContext
{
    public class FolderApiRepository : IRepository<FolderViewModel>
    {
        private IApiService _apiService;               
        public FolderApiRepository(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task CreateAsync(FolderViewModel item)
        {
            await _apiService.AddFolderAsync(item);
        }

        public async Task DeleteAsync(FolderViewModel item)
        {
            await _apiService.DeleteFolderAsync(item.Id);
        }


        public async Task<FolderViewModel> GetAsync(FolderViewModel item)
        {
            var folder = new FolderViewModel();
            var files = await _apiService.GetFolderFilesAsync(item.Id);
            var folders = await _apiService.GetFolderFoldersAsync(item.Id);
            
            folder.Child = new ObservableCollection<IBaseVM>(files.Concat(folders));
            return folder;
        }

        public async Task<ObservableCollection<FolderViewModel>> GetList()
        {
            var baseFolder = await _apiService.GetFolderFilesAsync(1);
            return new ObservableCollection<FolderViewModel>();
            //return (baseFolder as FolderViewModel).Child;
        }


        public async Task UpdateAsync(FolderViewModel item)
        {
            await _apiService.UpdateFolderAsync(item);
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
