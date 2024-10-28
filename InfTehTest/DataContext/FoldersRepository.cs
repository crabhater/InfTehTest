using InfTehTest.InterfacesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.DataContext
{
    public class FoldersRepository : IRepository<TreeViewVM>
    {
        private IApiService _apiService;
        public Task CreateAsync(TreeViewVM item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TreeViewVM item)
        {
            throw new NotImplementedException();
        }


        public Task<TreeViewVM> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TreeViewVM>> GetList(Func<TreeViewVM, bool> func)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id)
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
