using FileCleanup.ViewModels;

namespace FileCleanup.Services
{
    public interface IDialogService
    {
        public T OpenDialog<T>(DialogViewModelBase<T> viewModel);
    }
}