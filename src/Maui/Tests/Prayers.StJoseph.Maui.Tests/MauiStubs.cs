// Minimal MAUI type stubs so non-UI source files compile under net9.0 in tests.
// These are only compiled when UNIT_TESTING is defined.
#if UNIT_TESTING

namespace Microsoft.Maui.Controls
{
    public class Application
    {
        public static Application? Current { get; set; }
        public ResourceDictionary? Resources { get; set; }
    }

    public class ResourceDictionary : Dictionary<string, object>
    {
        public new bool ContainsKey(string key) => base.ContainsKey(key);
    }

    public class Command : System.Windows.Input.ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        public Command(Action execute, Func<bool>? canExecute = null) { _execute = execute; _canExecute = canExecute; }
        public Command(Func<System.Threading.Tasks.Task> execute, Func<bool>? canExecute = null)
        { _execute = () => _ = execute(); _canExecute = canExecute; }
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object? parameter) => _execute();
        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class Command<T> : System.Windows.Input.ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;
        public Command(Action<T?> execute, Func<T?, bool>? canExecute = null) { _execute = execute; _canExecute = canExecute; }
        public Command(Func<T?, System.Threading.Tasks.Task> execute, Func<T?, bool>? canExecute = null)
        { _execute = p => _ = execute(p); _canExecute = canExecute; }
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;
        public void Execute(object? parameter) => _execute((T?)parameter);
        public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class Shell
    {
        public static Shell Current { get; } = new Shell();
        public System.Threading.Tasks.Task GoToAsync(string route, bool animate = true)
            => System.Threading.Tasks.Task.CompletedTask;
    }
}

namespace Microsoft.Maui
{
    public static class MainThread
    {
        public static bool IsMainThread => true;
        public static void BeginInvokeOnMainThread(Action action) => action();
    }

    public struct Color
    {
        public static Color FromArgb(string hex) => default;
    }
}

namespace Microsoft.Maui.Storage
{
    public static class FileSystem
    {
        public static System.Threading.Tasks.Task<System.IO.Stream> OpenAppPackageFileAsync(string filename)
            => System.Threading.Tasks.Task.FromResult<System.IO.Stream>(new System.IO.MemoryStream());
    }
}

#endif
