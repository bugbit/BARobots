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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreRobots.Sdk
{
    public class CSRobot : IInitCSRobot
    {
        private int mId;
        private IArena mArena;

        protected static T RunSynchronously<T>(Task<T> argTask)
        {
            var pEvent = new AutoResetEvent(false);
            var pAwaiter = argTask.ConfigureAwait(true).GetAwaiter();

            pAwaiter.OnCompleted(() => pEvent.Set());

            pEvent.WaitOne();

            return pAwaiter.GetResult();
        }


        protected async Task<int> loc_x_async()
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return (int)await mArena.loc_x(mId);
        }

        protected int loc_x() => RunSynchronously(loc_x_async());

        //protected int loc_x()
        //{
        //    mArena.CancelToken.ThrowIfCancellationRequested();

        //    return (int)mArena.LocX;
        //}

        //protected int loc_y()
        //{
        //    mArena.CancelToken.ThrowIfCancellationRequested();

        //    return (int)mArena.LocY;
        //}

        void IInitCSRobot.Init(int argId, IArena argArena)
        {
            mId = argId;
            mArena = argArena;
        }

        void IInitCSRobot.DeInit()
        {
            mArena = null;
        }
    }
}
