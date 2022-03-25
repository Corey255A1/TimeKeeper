using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

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
        public TimeTicker TimeTicker
        {
            get => _time_ticker;
        }

        private ChargeCodeTimer _currently_working;
        public ChargeCodeTimer CurrentlyWorkingChargeCode
        {
            get => _currently_working;
            set
            {
                _currently_working = value;
                NotifyChange(nameof(CurrentlyWorkingChargeCode));
            }
        }

        private bool _work_timer_running = false;
        public bool WorkTimerRunning
        {
            get => _work_timer_running;
            set
            {
                _work_timer_running = value;
                NotifyChange(nameof(WorkTimerRunning));
            }
        }

        private DateTime _start_datetime = new DateTime();
        public DateTime StartDateTime
        {
            get => _start_datetime;
            set
            {
                _start_datetime = value;
                NotifyChange(nameof(StartDateTime));
                NotifyChange(nameof(StartTime));
            }
        }
        public MutableTime StartTime
        {
            get => new MutableTime(_start_datetime);
        }

        private DateTime _current_datetime = DateTime.Now;
        public DateTime CurrentDateTime
        {
            get => _current_datetime;
            set
            {
                _current_datetime = value;
                NotifyChange(nameof(CurrentDateTime));
                NotifyChange(nameof(CurrentTime));
            }
        }
        public MutableTime CurrentTime
        {
            get => new MutableTime(_current_datetime);
        }

        public MutableTime DeltaTime
        {
            get => _current_datetime - _start_datetime;
        }

        public MutableTime TotalWorkTime
        {
            get
            {
                var time_span = new MutableTime();
                foreach (var charge_code in _time_card.ChargeCodes)
                {
                    time_span += charge_code.Time;
                }
                return time_span;
            }
        }

        private Dispatcher _dispatcher;
        public TimeCardController(Dispatcher dispatcher, string initial_load_path)
        {
            TimeCard = new TimeCard(initial_load_path);
            _dispatcher = dispatcher;
            _time_ticker.TickEvent += _time_ticker_TickEvent;
        }

        private void _time_ticker_TickEvent(DateTime time, TimeSpan elapsed)
        {
            _dispatcher.Invoke(()=> {
                CurrentDateTime = time;
                if (WorkTimerRunning) CurrentlyWorkingChargeCode?.Time.IncrementTime(elapsed);
                NotifyChange(nameof(DeltaTime));
                NotifyChange(nameof(TotalWorkTime));
            });
        }

        public void Reset()
        {
            _time_card.Reset();
            NotifyChange(nameof(TotalWorkTime));
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

        public void SetStartTime(int hour, int minute, int second)
        {
            _start_datetime = new DateTime(_start_datetime.Year, _start_datetime.Month, _start_datetime.Day, hour, minute, second);
        }
    }
}
