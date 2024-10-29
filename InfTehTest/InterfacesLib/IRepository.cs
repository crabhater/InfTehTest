using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.InterfacesLib
{
    public interface IRepository<T> : IDisposable
        where T : class
    {

        Task<ObservableCollection<T>> GetList();
        Task<T> GetAsync(T item);
        Task CreateAsync(T item);
        Task DeleteAsync(T item);
        Task UpdateAsync(T item);
    }
}
