using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.Models
{
    public class TimeCardController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private TimeCard _time_card;
        public TimeCard TimeCard
        {
            get => _time_card;
            set
            {
                _time_card = value;
                NotifyChange(nameof(TimeCard));
            }
        }

        private TimeTicker _time_ticker = new TimeTicker();

        private ChargeCodeTimer _currently_working;
        public ChargeCodeTimer CurrentlyWorkingChargeCode
        {
            get => _currently_working;
            set
            {
                _currently_working = value;
            }
        }

        private bool _work_timer_running = false;
        public bool WorkTimerRunning
        {
            get => _work_timer_running;
            set
            {
                _work_timer_running = value;
            }
        }

        private DateTime _start_time = new DateTime();
        public DateTime StartTime
        {
            get => _start_time;
            set
            {
                _start_time = value;
            }
        }

        private DateTime _current_time = DateTime.Now;
        public DateTime CurrentTime
        {
            get => _current_time;
            set
            {
                _current_time = value;
            }
        }

        public TimeCardController(string initial_load_path)
        {
            TimeCard = new TimeCard(initial_load_path);
        }

        public void AddNewChargeCode()
        {
            _time_card.AddNewChargeCode();
        }
        public void RemoveChargeCode(ChargeCodeTimer charge_code)
        {
            _time_card.RemoveChargeCode(charge_code);
        }

        public void WorkOnChargeCode(ChargeCodeTimer charge_code)
        {
            CurrentlyWorkingChargeCode = charge_code;
        }
    }
}
