using InfTehTest.InterfacesLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfTehTest.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static async Task<bool> FindAndDoActionAsync(this ObservableCollection<TreeViewVM> viewModels, TreeViewVM viewModel, Action<TreeViewVM> action = null)
        {
            return await viewModels.CheckChildsAndDoAsync(e => e.TypeId == viewModel.TypeId && e.Id == e.Id, action);
        }

        private static async Task<bool> CheckChildsAndDoAsync(this ObservableCollection<TreeViewVM> viewModels, Func<TreeViewVM, bool> predicate, Action<TreeViewVM> action)
        {
            if (viewModels != null && viewModels.Count > 0)
            {
                foreach (var vm in viewModels)
                {
                    if (predicate(vm))
                    {
                        action(vm);
                        return true;
                    }
                    else
                    {
                        var result = await vm.Child.CheckChildsAndDoAsync(predicate, action);
                        return result;
                    }
                }
            }
            return false;
        }
    }
}
