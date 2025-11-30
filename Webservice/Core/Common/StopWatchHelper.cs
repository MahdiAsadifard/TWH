using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.Common
{
    public class StopWatchHelper : IDisposable
    {
        Stopwatch _stopwatch = null;
        public StopWatchHelper()
        {
            _stopwatch = new Stopwatch();
            this.Start();
        }

        public void Dispose()
        {
            this.Stop();
            _stopwatch = null;
        }

        public void Start() => _stopwatch.Start();

        public void Stop() => _stopwatch.Stop();

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

    }
}
