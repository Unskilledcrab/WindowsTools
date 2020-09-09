using System.ComponentModel;

namespace FileCleanup.Models
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
