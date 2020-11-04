using FileCleanup.Services;
using FileCleanupDataLib.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace FileCleanup.ViewModels
{
    public class ScanProfileDialogViewModel : DialogViewModelBase<CfgScanProfile>
    {
        public RelayCommand<IDialogWindow> CreateCommand { get; }
        public RelayCommand<IDialogWindow> CancelCommand { get; }

        public CfgScanProfile ScanProfile { get; set; } = new CfgScanProfile();

        public ScanProfileDialogViewModel(string windowTitle, string message) : base(windowTitle, message)
        {
            CreateCommand = new RelayCommand<IDialogWindow>(Create);
            CancelCommand = new RelayCommand<IDialogWindow>(Cancel);
        }

        private void Create(IDialogWindow window) => CloseDialogWithResult(window, ScanProfile);
        private void Cancel(IDialogWindow window) => CloseDialogWithResult(window, null);
    }
}