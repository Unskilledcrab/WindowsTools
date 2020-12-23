using System.Windows.Controls;
using WT.FileScanner.WPF.ViewModels;

namespace WT.FileScanner.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for FileScanner.xaml
    /// </summary>
    public partial class FileScanner : UserControl
    {
        public FileScanner(FileScannerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}