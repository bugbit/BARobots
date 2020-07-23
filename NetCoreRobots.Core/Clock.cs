#region License

/*
MIT License

Autor Oscar Hernández Bañó
Copyright (c) 2020 bugbit

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace NetCoreRobots.Core
{
    public class Clock
    {
        //http://stackoverflow.com/questions/1416139/how-to-get-timestamp-of-tick-precision-in-net-c
        private DateTime _startTime;
        private Stopwatch _stopWatch;
        private readonly TimeSpan MaxIdle = TimeSpan.FromSeconds(10);

        private long mTicksStartGame;
        private long? mTicksLastUpdate;

        public double Speed { get; set; } = 10;

        public double Elapsed { get; private set; } // s
        public double ElapsedGame => GetElapsedSeconds(Ticks, mTicksStartGame);  // s

        public void StartGame() => mTicksStartGame = Ticks;
        public void StartUpdate()
        {
            var pTicks = Ticks;

            Elapsed = (mTicksLastUpdate.HasValue) ? GetElapsedSeconds(pTicks, mTicksLastUpdate.Value) : 0;
            mTicksLastUpdate = pTicks;
        }

        private double GetElapsedSeconds(long argTickInit, long argTickEnd) => (double)(argTickEnd - argTickInit) * Speed / Stopwatch.Frequency;

        //http://stackoverflow.com/questions/1416139/how-to-get-timestamp-of-tick-precision-in-net-c
        private long Ticks
        {
            get
            {
                if (_stopWatch == null || _startTime.Add(MaxIdle) < DateTime.UtcNow)
                    Reset();
                return _startTime.Ticks + _stopWatch.Elapsed.Ticks;
            }
        }

        private void Reset()
        {
            _startTime = DateTime.UtcNow;
            _stopWatch = Stopwatch.StartNew();
        }
    }
}
