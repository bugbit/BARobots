﻿#region License

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
using System.Linq;
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
        public Func<Task> Display { get; set; }

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
            var pidTeam = 1;

            foreach (var r in mRobotsToMatch)
            {
                if (r.Item1 == 1)
                    CreateRobot(r.Item2, null, null);
                else
                {
                    for (var pIdTeamMember = r.Item1; pIdTeamMember >= 1; pIdTeamMember--)
                        CreateRobot(r.Item2, pidTeam, pIdTeamMember);
                }
            }
        }

        private RobotInfo CreateRobot(string argName, int? argIdTeam, int? argIdMemberTeam)
        {
            var pRobot = FactoryRobots.Create(argName);

            lock (mRobots)
            {
                var pId = mRobots.Count;
                var pInfo = new RobotInfo
                {
                    CSRobot = pRobot.Item1,
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
