using System;
using System.Threading.Tasks;
using System.Windows;
using WT.FileScanner.UI.Shared.Services.Interfaces;

namespace WT.FileScanner.WPF.Services
{
    public class UIManager : IUIManager
    {
        public void ShowAlert(string message)
        {
            MessageBox.Show(message);
        }

        public Task ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}