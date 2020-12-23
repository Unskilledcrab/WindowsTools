using System;
using System.Threading;
using System.Threading.Tasks;
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

        public override Task OnUpdate(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}