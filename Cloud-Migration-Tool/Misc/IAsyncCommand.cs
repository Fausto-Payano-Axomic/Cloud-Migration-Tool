using Cloud_Migration_Tool.Misc;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cloud_Migration_Tool.Helper_Classes {
    public interface IAsyncCommand : IAsyncCommand<object> {
    }

    public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged {
        Task ExecuteAsync(T obj);
        bool CanExecute(object obj);
        ICommand Command { get; }
    }
}