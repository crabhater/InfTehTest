using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
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
        public static async Task<bool> FindAndDoActionAsync(this ObservableCollection<IBaseVM> viewModels, IBaseVM viewModel, Action<IBaseVM> action = null)
        {
            return await viewModels.CheckChildsAndDoAsync( e => e.Id == viewModel.Id && e.GetType() == viewModel.GetType(), action);
        }
        //public static async Task<bool> ContainsKey(this ObservableCollection<IBaseVM> viewModels, IBaseVM viewModel)
        //{
        //    return await CheckChildsAndDoAsync(viewModels, e => e.Id == viewModel.Id && e.TypeId == viewModel.TypeId, a => { });
        //}

        private static async Task<bool> CheckChildsAndDoAsync(this ObservableCollection<IBaseVM> viewModels, Func<IBaseVM, bool> predicate, Action<IBaseVM> action)
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
                        try
                        {
                            var result = await (vm as FolderViewModel).Child.CheckChildsAndDoAsync(predicate, action);
                            if (result)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return false;
        }
    }
}
