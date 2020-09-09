using FileCleanup.ViewModels;
using System;
using System.Windows.Input;

namespace FileCleanup.Commands
{
    public class CancelScanCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainWindowViewModel VM { get; set; }

        public CancelScanCommand(MainWindowViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.CancelScanner();
        }
    }
}
