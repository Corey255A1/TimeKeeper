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
            theTicker = new Timer(Tick, this, 1000, 1000); 
        }
        private void Tick(object obj)
        {
            //If(TickEvent != null) TickEvent(DateTime.UtcNow);
            TickEvent?.Invoke(DateTime.Now);
        }
    }
}
