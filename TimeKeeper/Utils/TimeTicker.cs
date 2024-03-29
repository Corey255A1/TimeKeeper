﻿//Corey Wunderlich 2018
//A simple class to tick all of the elements and keep the clocks in sync
//Even between computer sleeps or lock ups. It uses time stamps in the callback
//to allow for time offsets
using System;
using System.Threading;
namespace TimeKeeper
{
    public delegate void TickCallback(DateTime time, TimeSpan elapsed);
    public class TimeTicker
    {
        public event TickCallback TickEvent;

        private DateTime _lastTimeReceived = DateTime.MinValue;
        Timer _ticker;
        public TimeTicker()
        {
            //wait until the nearst second for the first tick
            _ticker = new Timer(Tick, this, 1000 - DateTime.Now.Millisecond, 1000);
            _lastTimeReceived = DateTime.Now;
        }
        private void Tick(object obj)
        {
            var now = DateTime.Now;
            TickEvent?.Invoke(now, now - _lastTimeReceived);
            _ticker.Change(1000 - now.Millisecond, 1000); //Call it again to account for delay in callbacks
            _lastTimeReceived = now;
        }
    }
}
