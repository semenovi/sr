
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
        private static long _ticksStart = 0;
        private static Stopwatch _stopWatch = null;
        
        public static long ticks
        {
            get
            {
                return _stopWatch.Elapsed.Ticks - _ticksStart;
            }
        }
        public static void start()
        {
            _stopWatch = Stopwatch.StartNew();
            _ticksStart = _stopWatch.Elapsed.Ticks;
        }
    }
}