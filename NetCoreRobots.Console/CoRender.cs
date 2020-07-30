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

using NetCoreRobots.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

using static System.Console;

namespace NetCoreRobots.Console
{
    // https://en.wikipedia.org/wiki/Box-drawing_character
    class CoRender
    {
        const int StatusBoxesWitdh = 20;
        const int SepFieldStatusBoxesWitdh = 3;

        private readonly CoSize PanelRobot = new CoSize { w = StatusBoxesWitdh + SepFieldStatusBoxesWitdh, h = 5 };

        private bool mCursorVisible0;
        private CoRect mArenaR;
        private string mField1;
        private string mField2;
        private ImmutableArray<CoRobotInfo> mRobots;

        public CoSize WindowS { get; private set; }

        public void Init(Arena argArena)
        {
            var w = 80;
            var h = 25; // 60
            //var w1 = LargestWindowWidth - 2 - SepFieldStatusBoxesWitdh - StatusBoxesWitdh;
            //var h1 = LargestWindowHeight - 2;

            mCursorVisible0 = CursorVisible;
            WindowS = new CoSize { w = w, h = h };
            mArenaR = new CoRect
            {
                p = new CoPos { x = 1, y = 1 },
                s = new CoSize { w = w - StatusBoxesWitdh - SepFieldStatusBoxesWitdh - 2, h = h - 2 }
            };
            mField1 = $"┌{new string('─', mArenaR.s.w)}┐";
            mField2 = $"└{new string('─', mArenaR.s.w)}┘";
            mRobots = (from r in argArena.Robots select new CoRobotInfo { RobotInfo = r }).ToImmutableArray();
            CursorVisible = false;
            SetWindowSize(WindowS.w, WindowS.h);
            SetCursorPosition(0, 0);
            SetBufferSize(WindowS.w, WindowS.h);
            Clear();
            DisplayField(argArena);
        }
        public void Display(Arena argArena)
        {
            SetCursorPosition(mArenaR.p.x + mArenaR.s.w + 2, mArenaR.p.y);
            Write($"{argArena.GetTime():n2}    ");
            CalcRobotsPos(argArena);
            CleanRobotsScreen();
            DisplatRobots();
        }

        public void End(Arena argArena)
        {
            SetCursorPosition(0, mArenaR.p.y + mArenaR.s.h);
            CursorVisible = mCursorVisible0;
        }

        private void DisplayField(Arena argArena)
        {
            int pW1 = mArenaR.p.x + mArenaR.s.w;

            SetCursorPosition(0, 0);
            WriteLine(mField1);
            for (var y = 1; y <= mArenaR.s.h; y++)
            {
                Write('│');
                SetCursorPosition(pW1, y);
                WriteLine('│');
            }
            Write(mField2);
        }

        private void CalcRobotsPos(Arena argArena)
        {
            foreach (var r in mRobots)
            {
                var ri = r.RobotInfo;

                r.SetPos(mArenaR.p.x + (int)ri.LocX * mArenaR.s.w / argArena.MaxX, mArenaR.p.y + (int)(mArenaR.s.h * (argArena.MaxY - ri.LocY - 1) / argArena.MaxY));
            }
        }

        private void CleanRobotsScreen()
        {
            foreach (var r in mRobots)
            {
                var pPos = r.PosAnt;
                var pPosAct = r.Pos;

                if (pPos != null && (pPos.x != pPosAct.x || pPos.y != pPosAct.y))
                {
                    SetCursorPosition(pPos.x, pPos.y);
                    Write(' ');
                }
            }
        }

        private void DisplatRobots()
        {
            foreach (var r in mRobots)
            {
                var pPos = r.Pos;

                SetCursorPosition(pPos.x, pPos.y);
                Write(r.RobotInfo.IdTeamOrRobot.ToString());
                r.ActPos();
            }
        }
    }
}
