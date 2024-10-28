using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.InterfacesLib
{
    internal interface IRepository<T> : IDisposable
        where T : class
    {
        Task<ICollection<T>> GetList(Func<T, bool> func);
        Task<T> GetAsync(int id);
        Task CreateAsync(T item);
        Task DeleteAsync(T item);
        Task UpdateAsync(int id);
        Task SaveAsync();
    }
}
