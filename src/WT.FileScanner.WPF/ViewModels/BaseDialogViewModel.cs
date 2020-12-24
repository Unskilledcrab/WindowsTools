using WT.FileScanner.UI.Shared.ViewModels;

namespace WT.FileScanner.WPF.ViewModels
{
    public class BaseDialogViewModel<T> : BaseViewModel
    {
        public T Result { get; set; }

        public void CloseDialogWithResult(T result)
        {
            Result = result;
        }
    }
}