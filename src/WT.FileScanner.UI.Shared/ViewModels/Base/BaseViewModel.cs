using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WT.FileScanner.UI.Shared.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        public string Title { get; set; } = "No Title";

        protected void ShowAlert(string message) => IoC.UI.ShowAlert(message);
    }
}