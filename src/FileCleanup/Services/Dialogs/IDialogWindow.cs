namespace FileCleanup.Services
{
    public interface IDialogWindow
    {
        bool? DialogResult { get; set; }
        object DataContext { get; set; }
        public bool? ShowDialog();
    }
}