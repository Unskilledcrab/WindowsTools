using System.Threading.Tasks;

namespace WT.FileScanner.UI.Shared.Services.Interfaces
{
    public interface IUIManager
    {
        Task ShowDialog();

        void ShowAlert(string message);
    }
}