using System;
using System.Windows.Input;

namespace SP.Shell.Commands
{
    public class GuiListenCommand : ICommand
    {
        private readonly Action<object> execute;

        private readonly Func<object, bool> canExecute; 

        public GuiListenCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}