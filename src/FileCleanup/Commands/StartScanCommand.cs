using FileCleanup.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FileCleanup.Commands
{
    public class StartScanCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainWindowViewModel VM { get; set; }

        public StartScanCommand(MainWindowViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.StartScanner();
        }
    }

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
