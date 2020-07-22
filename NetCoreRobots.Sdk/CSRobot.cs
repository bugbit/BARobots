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

        private static T RunSynchronously<T>(Task<T> argTask)
        {
            var pEvent = new AutoResetEvent(false);
            var pAwaiter = argTask.ConfigureAwait(true).GetAwaiter();

            pAwaiter.OnCompleted(() => pEvent.Set());

            pEvent.WaitOne();

            return pAwaiter.GetResult();
        }

        private static void RunSynchronously(Task argTask)
        {
            var pEvent = new AutoResetEvent(false);
            var pAwaiter = argTask.ConfigureAwait(true).GetAwaiter();

            pAwaiter.OnCompleted(() => pEvent.Set());

            pEvent.WaitOne();
        }

        /// <summary>
        /// The speed() function returns the current speed of the robot. speed() takes no arguments, and returns the percent of speed, 0-100. Note that speed() may not always be the same as the last drive(), because of acceleration and deceleration.
        ///
        ///Examples:
        ///drive(270,100);   /* start drive, due south */
        ///; ; ;             /* other instructions */
        ///if (speed() == 0) /* check current speed */
        ///{
        ///drive(90,20);   /* ran into the south wall, or another robot */
        ///}
        /// </summary>
        /// <param name="argIdRobot"></param>
        /// <returns></returns>
        protected async Task<int> speed_async()
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return (int)await mArena.speed(mId);
        }

        /// <summary>
        /// The speed() function returns the current speed of the robot. speed() takes no arguments, and returns the percent of speed, 0-100. Note that speed() may not always be the same as the last drive(), because of acceleration and deceleration.
        ///
        ///Examples:
        ///drive(270,100);   /* start drive, due south */
        ///; ; ;             /* other instructions */
        ///if (speed() == 0) /* check current speed */
        ///{
        ///drive(90,20);   /* ran into the south wall, or another robot */
        ///}
        /// </summary>
        /// <param name="argIdRobot"></param>
        /// <returns></returns>
        protected int speed() => RunSynchronously(speed_async());

        /// <summary>
        /// function returns the robot's current x axis location. loc_x() takes no arguments, and returns 0-999.
        /// </summary>
        /// <returns></returns>
        protected async Task<int> loc_x_async()
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return (int)await mArena.loc_x(mId);
        }

        /// <summary>
        /// function returns the robot's current x axis location. loc_x() takes no arguments, and returns 0-999.
        /// </summary>
        /// <returns></returns>
        protected int loc_x() => RunSynchronously(loc_x_async());

        /// <summary>
        ///  The loc_y() function is similar to loc_x(), but returns the current y axis position.
        /// </summary>
        /// <returns></returns>
        protected async Task<int> loc_y_async()
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return (int)await mArena.loc_y(mId);
        }

        /// <summary>
        ///  The loc_y() function is similar to loc_x(), but returns the current y axis position.
        /// </summary>
        /// <returns></returns>
        protected int loc_y() => RunSynchronously(loc_y_async());

        /// <summary>
        /// The rand() function returns a random number between 0 and limit, up to 32767.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected int rand(int limit)
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return mArena.rand(limit);
        }

        /// <summary>
        /// The sqrt() returns the square root of a number. Number is made positive, if necessary.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int sqrt(int value)
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return mArena.sqrt(value);
        }

        /// <summary>
        ///  atan() takes a ratio argument that has been scaled up by 100,000, and returns a degree value, between -90 and +90
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int atan(int value)
        {
            if (mArena == null)
                throw new TaskCanceledException();

            return mArena.atan(value);
        }

        /// <summary>
        /// function activates the robot's drive mechanism, on a
        /// specified heading and speed.Degree is forced into the range
        /// 0-359 as in scan().  Speed is expressed as a percent, with 100 as
        /// maximum.A speed of 0 disengages the drive.Changes in
        /// direction can be negotiated at speeds of less than 50 percent.
        /// Examples:
        /// drive(0,100);  /* head due east, at maximum speed */
        /// drive(90,0);   /* stop motion */
        /// </summary>
        /// <param name="degrees">is the direction of movement of the robot (angles start from 3 o'clock and increase clockwise). You must remember that robots can change their direction only if the speed is lower that 50% (say 15 m/s).</param>
        /// <param name="speed">is the speed in percent that the robot must reach: 0 means 0 m/s, 100 means 30 m/s.</param>
        /// <returns></returns>

        protected async Task drive_async(int degrees, int speed)
        {
            if (mArena == null)
                throw new TaskCanceledException();

            await mArena.drive(mId, degrees, speed);
        }

        /// <summary>
        /// function activates the robot's drive mechanism, on a
        /// specified heading and speed.Degree is forced into the range
        /// 0-359 as in scan().  Speed is expressed as a percent, with 100 as
        /// maximum.A speed of 0 disengages the drive.Changes in
        /// direction can be negotiated at speeds of less than 50 percent.
        /// Examples:
        /// drive(0,100);  /* head due east, at maximum speed */
        /// drive(90,0);   /* stop motion */
        /// </summary>
        /// <param name="degrees">is the direction of movement of the robot (angles start from 3 o'clock and increase clockwise). You must remember that robots can change their direction only if the speed is lower that 50% (say 15 m/s).</param>
        /// <param name="speed">is the speed in percent that the robot must reach: 0 means 0 m/s, 100 means 30 m/s.</param>
        /// <returns></returns>
        protected void drive(int degrees, int speed) => RunSynchronously(drive_async(degrees, speed));

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
