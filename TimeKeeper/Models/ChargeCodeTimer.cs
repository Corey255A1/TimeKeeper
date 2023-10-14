//Corey Wunderlich WunderVision 2022
//The Properties of the ChargeCodeTimer
using System.ComponentModel;

namespace TimeKeeper.Models
{
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

        public ChargeCodeTimer(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
