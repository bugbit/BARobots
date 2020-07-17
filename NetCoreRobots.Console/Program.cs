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
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace NetCoreRobots.Console
{
    // https://en.wikipedia.org/wiki/Box-drawing_character
    class Program
    {
        const int StatusBoxesWitdh = 20;
        const int SepFieldStatusBoxesWitdh = 3;

        static Arena mArena = new Arena
        {
            Display = DisplayArena,
            FactoryRobots = new FactoryRobots(typeof(Program).Assembly)
        };

        readonly static CoSize PanelRobot = new CoSize { w = StatusBoxesWitdh + SepFieldStatusBoxesWitdh, h = 5 };

        static CoRect WindowOrig;
        static CoSize WindowS;
        static CoRect ArenaR;
        static int ArenaWidth;
        static int ArenaHeight;
        static int XPanelRobot;
        static int YPanelRobot;

        static async Task Main(string[] args)
        {
            CancelKeyPress += (s, e) => { e.Cancel = true; Cancel(); };
            mArena.CrearRobotsToMatchSolo("RobotTest1");
            mArena.InitMatch();
            InitScreen();
            DisplayScreen();
            await mArena.StartMatch();
            //OutputEncoding = Encoding.GetEncoding(28591);

            //for (var i = 0; i < 512; i++)
            //    Write($"{i} = {(char)i}\t");

            End();
            ReadLine();
        }

        private static void GetWindowRect(out CoRect r)
        {
            r = new CoRect { x = WindowTop, y = WindowLeft, s = new CoSize { w = BufferWidth, h = BufferHeight } };
        }

        private static void InitScreen()
        {
            GetWindowRect(out WindowOrig);

            var w1 = LargestWindowWidth - 2 - SepFieldStatusBoxesWitdh - StatusBoxesWitdh;
            var h1 = LargestWindowHeight - 2;

            CalcWindowSize(out int w, out int h);
            WindowS = new CoSize { w = w, h = h };
            SetWindowSize(w, h);
            SetBufferSize(w, h);
            ArenaR = new CoRect
            {
                x = 1,
                y = 1,
                s = new CoSize { w = w - StatusBoxesWitdh - SepFieldStatusBoxesWitdh - 2, h = h - 2 }
            };
        }

        static void CalcWindowSize(out int w, out int h)
        {
            //LargestWindowHeight
            //LargestWindowWidth
            w = 80;
            h = 60;
        }

        static void SetWindowRect(CoRect r)
        {
            SetBufferSize(r.s.w, r.s.h);
            SetWindowPosition(r.x, r.y);
        }

        static void DisplayScreen()
        {
            var pCursor = CursorVisible;

            CursorVisible = false;
            try
            {
                Clear();
                DisplayField();
            }
            finally
            {
                CursorVisible = pCursor;
            }
        }

        static void DisplayField()
        {
            int pW1 = ArenaR.x + ArenaR.s.w;

            SetCursorPosition(0, 0);
            Write('┌');
            for (var x = 1; x < ArenaR.s.w; x++)
                Write('─');
            WriteLine('┐');
            for (var y = 1; y < ArenaR.s.h; y++)
            {
                Write('│');
                SetCursorPosition(pW1, y);
                WriteLine('│');
            }
            Write('└');
            for (var x = 1; x < pW1; x++)
                Write('─');
            WriteLine('┘');
        }

        static Task DisplayArena() => Task.Run(DisplayArenaInternal);

        static void DisplayArenaInternal()
        {
            var pIsCursor = CursorVisible;
            GetWindowRect(out CoRect r);

            try
            {
                SetWindowRect(ArenaR);
                foreach (var pRobot in mArena.Robots)
                {
                    SetCursorPosition((int)pRobot.LocX * ArenaR.s.w / mArena.MaxX, (int)pRobot.LocY * ArenaR.s.h / mArena.MaxY);
                    Write(pRobot.IdTeamOrRobot.ToString());
                }
            }
            finally
            {
                SetWindowRect(r);
                pIsCursor = CursorVisible;

            }
            //SetWindowRect()
        }

        static void DisplayPanelRobots()
        {
            //buff
        }

        static void Cancel()
        {
            mArena.CancelMatch();
        }

        static void End()
        {
            SetWindowRect(WindowOrig);
            SetCursorPosition(0, ArenaR.y + ArenaR.s.h);
        }
    }
}
