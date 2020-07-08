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
using System.Threading;
using NetCoreRobots.Sdk;

namespace NetCoreRobots.Core
{
    public class Arena : IArena
    {
        public const int MaxRobots = 8;

        private CancellationTokenSource mCancelToken = new CancellationTokenSource();
        private List<RobotInfo> mRobots = new List<RobotInfo>();

        public FactoryRobots FactoryRobots { get; set; }

        public bool AddRobot(string argName)
        {
            var pRobot = FactoryRobots.Create(argName);

            if (pRobot.Item1 == null || pRobot.Item2 == null)
                return false;

            lock (mRobots)
            {
                var pId = mRobots.Count;
                var pInfo = new RobotInfo
                {
                    Name = argName,
                    Main = pRobot.Item2,
                    CancelToken = CancellationTokenSource.CreateLinkedTokenSource(mCancelToken.Token)
                };

                ((IInitCSRobot)pRobot.Item1).Init(pId, this);
                mRobots.Add(pInfo);
            }

            return true;
        }
    }
}
