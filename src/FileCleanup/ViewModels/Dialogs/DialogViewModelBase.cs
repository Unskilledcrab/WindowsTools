using FileCleanup.Services;
using GalaSoft.MvvmLight;

namespace FileCleanup.ViewModels
{
    public class DialogViewModelBase<T> : ViewModelBase
    {
        public string WindowTitle { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public DialogViewModelBase(string windowTitle, string message)
        {
            this.WindowTitle = windowTitle;
            this.Message = message;
        }
        public DialogViewModelBase(string windowTitle) : this(windowTitle, string.Empty) { }
        public DialogViewModelBase() : this(string.Empty, string.Empty) { }

        public void CloseDialogWithResult(IDialogWindow window, T result)
        {
            DialogResult = result;
            if (window != null)
                window.DialogResult = true;
        }
    }
}