using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.Models
{
    public class ChargeCodeTimer: INotifyPropertyChanged
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
