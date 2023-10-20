//Corey Wunderlich WunderVision 2022
//The Properties of the ChargeCodeTimer
using System.ComponentModel;
using System.Windows.Input;
using TimeKeeper.Utils;

namespace TimeKeeper.Models
{
    public delegate void ChargeCodeCallback(ChargeCodeTimer timer);
    public class ChargeCodeTimer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string _code;
        public string Code
        {
            get => _code;
            set { _code = value; NotifyChange(nameof(Code)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; NotifyChange(nameof(Description)); }
        }

        private bool _active;
        public bool Active
        {
            get => _active;
            set { _active = value; NotifyChange(nameof(Active)); }
        }

        private MutableTime _time = new MutableTime();
        public MutableTime Time
        {
            get => _time;
        }

        public ICommand RemoveCommand { get; private set; }
        public ICommand WorkOnCommand { get; private set; }

        public event ChargeCodeCallback Removed;
        public event ChargeCodeCallback WorkOn;

        public ChargeCodeTimer(string code, string description)
        {
            Code = code;
            Description = description;

            RemoveCommand = new GenericCommand(() =>
            {
                Removed?.Invoke(this);
            });

            WorkOnCommand = new GenericCommand(() =>
            {
                WorkOn?.Invoke(this);
            });
        }
    }
}
