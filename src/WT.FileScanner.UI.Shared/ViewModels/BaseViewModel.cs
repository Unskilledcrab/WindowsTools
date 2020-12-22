using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WT.FileScanner.UI.Shared.ViewModels
{
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        private CancellationTokenSource updateTokenSource = new CancellationTokenSource();

        public BaseViewModel()
        {
            UpdateCommand = new AsyncRelayCommand(Update);
            CancelUpdateCommand = new RelayCommand(CancelUpdate);
        }

        public event EventHandler BeforeUpdate;

        public event EventHandler AfterUpdate;

        public string Title { get; set; } = "No Title";
        public ICommand CancelUpdateCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public bool IsBusy { get; set; } = false;

        public abstract Task OnUpdate(CancellationToken token);

        public virtual async Task OnUpdateCancelled()
        {
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (updateTokenSource != null)
            {
                updateTokenSource.Dispose();
                updateTokenSource = null;
            }
        }

        protected async Task Update()
        {
            IsBusy = true;
            ResetCancellationToken();
            var token = updateTokenSource.Token;
            try
            {
                BeforeUpdate?.Invoke(this, EventArgs.Empty);
                await OnUpdate(token);
                AfterUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch (OperationCanceledException)
            {
                // log that the operation was cancelled here. this will have to be handled in each
                // viewmodel differently but the main log should go here
                await OnUpdateCancelled();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void CancelUpdate()
        {
            updateTokenSource.Cancel();
        }

        private void ResetCancellationToken()
        {
            updateTokenSource.Cancel();
            updateTokenSource.Dispose();
            updateTokenSource = new CancellationTokenSource();
        }
    }
}