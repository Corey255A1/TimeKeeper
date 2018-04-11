using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace TimeKeeper
{
    public delegate void TickCallback(DateTime time);
    class TimeTicker
    {
        public event TickCallback TickEvent;

        Timer theTicker;
        public TimeTicker()
        {
            //wait until the nearst second for the first tick
            theTicker = new Timer(Tick, this, 1000- DateTime.Now.Millisecond, 1000);
            
        }
        private void Tick(object obj)
        {
            //If(TickEvent != null) TickEvent(DateTime.UtcNow);
            TickEvent?.Invoke(DateTime.Now);
            theTicker.Change(1000 - DateTime.Now.Millisecond, 1000); //Call it again to account for delay in callbacks
        }
    }
}
