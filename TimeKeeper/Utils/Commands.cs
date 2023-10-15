//Corey Wunderlich WunderVision 2023
//https://www.wundervisionenvisionthefuture.com/
using System;
using System.Windows.Input;

namespace TimeKeeper.Utils
{

    public class GenericCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _callback;

        public GenericCommand(Action callback)
        {
            _callback = callback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _callback.Invoke();
        }
    }

    public class GenericCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> _callback;

        public GenericCommand(Action<T> callback)
        {
            _callback = callback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is T parameterT)
            {
                _callback.Invoke(parameterT);
            }

        }
    }

}
