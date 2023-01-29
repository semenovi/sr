
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;


namespace sr
{
    internal class hpc
    {
        private static DateTime _startTime;
        private static Stopwatch _stopWatch = null;
        private static TimeSpan _maxIdle =
            TimeSpan.FromSeconds(10);

        public static DateTime UtcNow
        {
            get
            {
                if ((_stopWatch == null) ||
                    (_startTime.Add(_maxIdle) < DateTime.UtcNow))
                {
                    Reset();
                }
                return _startTime.AddTicks(_stopWatch.Elapsed.Ticks);
            }
        }

        private static void Reset()
        {
            _startTime = DateTime.UtcNow;
            _stopWatch = Stopwatch.StartNew();
        }
    }
}