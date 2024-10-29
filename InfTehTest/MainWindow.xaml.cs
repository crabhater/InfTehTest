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

            if (sender is TreeViewItem item && item.DataContext is IBaseVM tvvm)
            {
                _viewModel.OpenFolder();
            }
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource?.DataContext == null || !originalSource.DataContext.Equals(((TreeViewItem)sender).DataContext))
                return;

            if (sender is TreeViewItem item && item.DataContext is IBaseVM tvvm)
            {
                _viewModel.SelectedItem = tvvm;
            }
        }

        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource?.DataContext == null || !originalSource.DataContext.Equals(((TextBox)sender).DataContext))
                return;

            if (sender is TextBox item && item.Visibility != Visibility.Collapsed)
            {
                await _viewModel.EndEdit(item.Text);
            }
        }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource?.DataContext == null || !originalSource.DataContext.Equals(((TextBox)sender).DataContext))
                return;

            if (sender is TextBox item && item.Visibility != Visibility.Collapsed)
            {
                if (e.Key == Key.Enter)
                {
                    await _viewModel.EndEdit(item.Text);
                    e.Handled = true;
                }
                if (e.Key == Key.Escape)
                {
                    await _viewModel.DropEdit();
                    e.Handled = true;
                }

            }
        }

    }
}