using FileCleanup.ViewModels;
using FileCleanup.Views;

namespace FileCleanup.Services
{
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(DialogViewModelBase<T> viewModel)
        {
            IDialogWindow window = new DialogView { DataContext = viewModel };
            window.ShowDialog();
            return viewModel.DialogResult;
        }
    }
}