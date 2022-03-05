//Corey Wunderlich 2018
//A simple class to tick all of the elements and keep the clocks in sync
//Even between computer sleeps or lock ups. It uses time stamps in the callback
//to allow for time offsets
using System;
using System.Threading;
namespace TimeKeeper
{
    public delegate void TickCallback(DateTime time);
    class TimeTicker
    {
        public event TickCallback TickEvent;

        Timer _ticker;
        public TimeTicker()
        {
            //wait until the nearst second for the first tick
            _ticker = new Timer(Tick, this, 1000 - DateTime.Now.Millisecond, 1000);

        }
        private void Tick(object obj)
        {
            //If(TickEvent != null) TickEvent(DateTime.UtcNow);
            TickEvent?.Invoke(DateTime.Now);
            _ticker.Change(1000 - DateTime.Now.Millisecond, 1000); //Call it again to account for delay in callbacks
        }
    }
}
