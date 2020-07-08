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
        public void AddRobot(string argName) => mRobotsToMatch.Add((1, argName));


        public void InitRobotsToMatchSolo(string argName)
        {
            MatchType = MatchTypes.Solo;
            mRobotsToMatch.Clear();
            AddRobot(argName);
        }

        public async Task StartMatch()
        {
            if (mMatchCancelToken != null)
                return;

            mMatchCancelToken = new CancellationTokenSource();

            InitMatch();

            var pTasks = new List<Task>(from r in mRobots select r.Main.Invoke());

            pTasks.Add(LoopMain());

            await Task.WhenAll(pTasks);

            mMatchCancelToken = null;
        }

        private void InitMatch()
        {
            InitMatchRobots();
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

        private async Task LoopMain()
        {
            var pToken = mMatchCancelToken.Token;

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
