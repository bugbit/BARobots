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
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using NetCoreRobots.Sdk;

namespace NetCoreRobots.Core
{
    public sealed class Arena : IArena
    {
        public const int MaxRobots = 8;

        private CancellationTokenSource mMatchCancelToken = null;
        private List<RobotInfo> mRobots = new List<RobotInfo>();
        private List<(int, string)> mRobotsToMatch = new List<(int, string)>();

        public ArenaStates State { get; set; } = ArenaStates.Created;
        public FactoryRobots FactoryRobots { get; set; }
        public MatchTypes MatchType { get; set; } = MatchTypes.Free;
        public RobotInfo[] Robots { get; private set; } = new RobotInfo[0];
        public Func<Task> Display { get; set; }
        public int MaxX { get; } = 1000;
        public int MaxY { get; } = 1000;

        public void AddTeamRobot(int argNumMembers, string argName) => mRobotsToMatch.Add((argNumMembers, argName));
        public void AddRobot(string argName)
        {
            int pNum;

            switch (MatchType)
            {
                case MatchTypes.Double:
                case MatchTypes.Double4:
                    pNum = 2;
                    break;
                case MatchTypes.Team:
                    pNum = 8;
                    break;
                default:
                    pNum = 1;
                    break;
            }

            mRobotsToMatch.Add((pNum, argName));
        }


        public void CrearRobotsToMatchSolo(string argName)
        {
            MatchType = MatchTypes.Solo;
            mRobotsToMatch.Clear();
            AddRobot(argName);
        }

        public void InitMatch()
        {
            if (State == ArenaStates.Created)
            {
                if (mMatchCancelToken == null)
                    mMatchCancelToken = new CancellationTokenSource();
                InitMatchRobots();
                State = ArenaStates.Initialized;
            }
        }

        public async Task StartMatch()
        {
            switch (State)
            {
                case ArenaStates.Created:
                case ArenaStates.Initialized:
                    break;
                default:
                    return;
            }

            switch (State)
            {
                case ArenaStates.Starting:
                case ArenaStates.Running:
                case ArenaStates.Stopped:
                case ArenaStates.Winner:
                    DeInitRobot();
                    State = ArenaStates.Created;
                    break;
            }

            InitMatch();

            foreach (var pTask in mRobots)
                pTask.Main.Invoke();

            try
            {
                await LoopMain();


            }
            finally
            {
                DeInitRobot();
                mMatchCancelToken = null;
                if (State != ArenaStates.Winner)
                    State = ArenaStates.Stopped;
            }
        }

        //private 

        public void CancelMatch()
        {
            mMatchCancelToken.Cancel();
        }

        /// <summary>
        /// function returns the robot's current x axis location. loc_x() takes no arguments, and returns 0-999.
        /// </summary>
        /// <param name="argIdRobot"></param>
        /// <returns></returns>
        async Task<double> IArena.loc_x(int argIdRobot)
        {
            if (argIdRobot >= mRobots.Count)
                throw new ArgumentOutOfRangeException();

            await Task.Yield();

            return mRobots[argIdRobot].LocX;
        }

        private void InitMatchRobots()
        {
            var pIdRobot = 1;
            var pIdTeamOrRobot = 1;
            var pIdTeam = 1;

            foreach (var r in mRobotsToMatch)
            {
                if (r.Item1 == 1)
                    CreateRobot(r.Item2, pIdRobot++, pIdTeamOrRobot++, null, null);
                else
                {
                    for (var pIdTeamMember = r.Item1; pIdTeamMember >= 1; pIdTeamMember--)
                        CreateRobot(r.Item2, pIdRobot++, pIdTeamOrRobot, pIdTeam, pIdTeamMember);
                    pIdTeamOrRobot++;
                }
            }

            Robots = mRobots.ToArray();
            InitPositionRobots();
        }

        private RobotInfo CreateRobot(string argName, int argIdRobot, int argIdTeamRobot, int? argIdTeam, int? argIdMemberTeam)
        {
            var pRobot = FactoryRobots.Create(argName);

            lock (mRobots)
            {
                var pId = mRobots.Count;
                var pInfo = new RobotInfo
                {
                    CSRobot = pRobot.Item1,
                    IdRobot = argIdRobot,
                    IdTeamOrRobot = argIdTeamRobot,
                    Name = argName,
                    IdTeam = argIdTeam,
                    IdMemberTeam = argIdMemberTeam,
                    Main = pRobot.Item2,
                    CancelToken = CancellationTokenSource.CreateLinkedTokenSource(mMatchCancelToken.Token)
                };

                ((IInitCSRobot)pRobot.Item1).Init(pId, this);
                mRobots.Add(pInfo);

                return pInfo;
            }
        }

        private void InitPositionRobots()
        {
            int pCol = 1;
            int pPow = 2;

            while (pPow < mRobots.Count)
            {
                pCol++;
                pPow *= pPow;
            }

            var pFilas2 = Math.DivRem(mRobots.Count, pCol, out int pRem);
            var pFilas = (pRem != 0) ? pFilas2 + 1 : pFilas2;
            var pRndShortCount = mRobots.Count * (1 + 2);
            var pRndBytes = new byte[2 * pRndShortCount];
            var pRndShort = new ushort[pRndShortCount];
            var w = MaxX / pCol;
            var h = MaxY / pFilas;
            var y = 0;
            var i = 0;
            var j = 0;

            new RNGCryptoServiceProvider().GetBytes(pRndBytes);
            Buffer.BlockCopy(pRndBytes, 0, pRndShort, 0, pRndBytes.Length);

            var pRobots2 = ShuffleRobot(mRobots, pRndShort, ref i);

            for (var f = 0; f < pFilas2; f++, y += h)
            {
                for (var x = 0; x < MaxX; x += w, j++)
                {
                    pRobots2[j].LocX = x + (pRndShort[i++] % w);
                    pRobots2[j].LocY = y + (pRndShort[i++] % h);
                }
            }
            if (pRem != 0)
                for (var x = 0; x < MaxX; x += w, j++)
                {
                    pRobots2[j].LocX = x + (pRndShort[i++] % w);
                    pRobots2[j].LocY = y + (pRndShort[i++] % h);
                }
        }

        private RobotInfo[] ShuffleRobot(IEnumerable<RobotInfo> argElems, ushort[] argRnds, ref int i)
        {
            var pShuffled = argElems.ToArray();

            for (int pIdx = pShuffled.Length - 1; pIdx > 0; pIdx--)
            {
                int pIdxSwap = argRnds[i++] % pIdx;
                var pTmp = pShuffled[pIdx];

                pShuffled[pIdx] = pShuffled[pIdxSwap];
                pShuffled[pIdxSwap] = pTmp;
            }

            return pShuffled.ToArray();
        }

        private void DeInitRobot()
        {
            foreach (var pRobot in mRobots)
                pRobot.CSRobot.DeInit();
            mRobots.Clear();
        }

        private async Task LoopMain()
        {
            var pToken = mMatchCancelToken.Token;

            State = ArenaStates.Running;
            for (; ; )
            {
                if (Display != null)
                    await Display.Invoke();
                if (pToken.IsCancellationRequested)
                    break;
            }
        }
    }
}
