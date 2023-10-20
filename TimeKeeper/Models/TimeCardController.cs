//Corey Wunderlich WunderVision 2022
//Ties together the active charge numbers and the current timers
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using TimeKeeper.Utils;
namespace TimeKeeper.Models
{
    public class TimeCardController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private TimeCard _timeCard;
        public TimeCard TimeCard
        {
            get => _timeCard;
            set
            {
                _timeCard = value;
                NotifyChange(nameof(TimeCard));
            }
        }

        public ICommand SetTimeCommand { get; set; }
        public ICommand PauseTimerCommand { get; set; }
        public ICommand ResetTimerCommand { get; set; }
        public ICommand AddNewChargeCodeCommand { get; set; }


        private TimeTicker _timeTicker = new TimeTicker();
        public TimeTicker TimeTicker
        {
            get => _timeTicker;
        }

        private ChargeCodeTimer _currentlyWorkingChargeCode;
        public ChargeCodeTimer CurrentlyWorkingChargeCode
        {
            get => _currentlyWorkingChargeCode;
            set
            {
                if (_currentlyWorkingChargeCode != null)
                {
                    _currentlyWorkingChargeCode.Active = false;
                }
                _currentlyWorkingChargeCode = value;
                if (_currentlyWorkingChargeCode != null)
                {
                    _currentlyWorkingChargeCode.Active = true;
                }
                NotifyChange(nameof(CurrentlyWorkingChargeCode));
            }
        }

        private bool _isWorkTimerRunning = false;
        public bool IsWorkTimerRunning
        {
            get => _isWorkTimerRunning;
            set
            {
                _isWorkTimerRunning = value;
                NotifyChange(nameof(IsWorkTimerRunning));
            }
        }

        private DateTime _startDateTime = new DateTime();
        public DateTime StartDateTime
        {
            get => _startDateTime;
            set
            {
                _startDateTime = value;
                NotifyChange(nameof(StartDateTime));
                NotifyChange(nameof(StartTime));
            }
        }
        public MutableTime StartTime
        {
            get => new MutableTime(_startDateTime);
        }

        private DateTime _currentDateTime = DateTime.Now;
        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                NotifyChange(nameof(CurrentDateTime));
                NotifyChange(nameof(CurrentTime));
            }
        }
        public MutableTime CurrentTime
        {
            get => new MutableTime(_currentDateTime);
        }

        public MutableTime DeltaTime
        {
            get => _currentDateTime - _startDateTime;
        }

        public MutableTime TotalWorkTime
        {
            get
            {
                var mutableTimeSpan = new MutableTime();
                foreach (var chargeCode in _timeCard.ChargeCodes)
                {
                    mutableTimeSpan += chargeCode.Time;
                }
                return mutableTimeSpan;
            }
        }

        private Dispatcher _dispatcher;
        public TimeCardController(Dispatcher dispatcher, string initialLoadPath)
        {
            TimeCard = new TimeCard(initialLoadPath);
            TimeCard.ChargeCodes.CollectionChanged += ChargeCodesCollectionChanged;
            _dispatcher = dispatcher;
            _timeTicker.TickEvent += TimeTickerTickEvent;


            SetTimeCommand = new GenericCommand(SetTime);
            PauseTimerCommand = new GenericCommand(PauseTimer);
            ResetTimerCommand = new GenericCommand(Reset);
            AddNewChargeCodeCommand = new GenericCommand(AddNewChargeCode);

            TimeCard.LoadRecentChargeNumbers();
        }

        private void ChargeCodesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (ChargeCodeTimer chargeCode in e.NewItems)
                {
                    chargeCode.Removed += RemoveChargeCode;
                    chargeCode.WorkOn += WorkOnChargeCode;
                }
            }
        }

        private void TimeTickerTickEvent(DateTime time, TimeSpan elapsed)
        {
            _dispatcher.Invoke(() =>
            {
                CurrentDateTime = time;
                if (IsWorkTimerRunning) CurrentlyWorkingChargeCode?.Time.IncrementTime(elapsed);
                NotifyChange(nameof(DeltaTime));
                NotifyChange(nameof(TotalWorkTime));
            });
        }


        public void SetTime()
        {
            var timeNow = DateTime.Now;
            StartDateTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, timeNow.Second); //truncate off any milliseconds
        }

        public void PauseTimer()
        {
            IsWorkTimerRunning = false;
        }

        public void Reset()
        {
            _timeCard.Reset();
            NotifyChange(nameof(TotalWorkTime));
        }

        public void AddNewChargeCode()
        {
            _ = _timeCard.AddNewChargeCode();
        }

        public void RemoveChargeCode(ChargeCodeTimer chargeCode)
        {
            _timeCard.RemoveChargeCode(chargeCode);
        }

        public void WorkOnChargeCode(ChargeCodeTimer chargeCode)
        {
            CurrentlyWorkingChargeCode = chargeCode;
            IsWorkTimerRunning = true;
        }

        public void AdjustStartTime(int hour, int minute, int second)
        {
            StartDateTime = new DateTime(_startDateTime.Year, _startDateTime.Month, _startDateTime.Day, hour, minute, second);
        }
    }
}
