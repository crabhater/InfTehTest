using InfTehTest.InterfacesLib;
using InfTehTest.ViewModel;
using InfTehTest.WebContext;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfTehTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()//MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            _viewModel = DataContext as MainViewModel;
            //DataContext = _viewModel;
        }

        private void TreeViewItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource?.DataContext == null || !originalSource.DataContext.Equals(((TreeViewItem)sender).DataContext))
                return;

            if (sender is TreeViewItem item && item.DataContext is TreeViewVM tvvm)
            {
                _viewModel.OpenFolder(tvvm);
            }
        }

        //private async void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        //{
        //    if (sender is TreeViewItem item && item.DataContext is FolderViewModel folder)
        //    {
        //        folder.LoadSubFoldersCommand.Execute(null);
        //    }
        //}

    }
}