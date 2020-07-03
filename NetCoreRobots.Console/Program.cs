using System;
using System.Text;
using static System.Console;

namespace NetCoreRobots.Console
{
    // https://en.wikipedia.org/wiki/Box-drawing_character
    class Program
    {
        const int StatusBoxesWitdh = 20;
        const int SepFieldStatusBoxesWitdh = 3;
        readonly static CoSize PanelRobot = new CoSize { w = StatusBoxesWitdh + SepFieldStatusBoxesWitdh, h = 5 };

        static CoSize WindowS;
        static CoRect ArenaR;
        static int ArenaWidth;
        static int ArenaHeight;
        static int XPanelRobot;
        static int YPanelRobot;

        static void Main(string[] args)
        {
            InitScreen();
            //OutputEncoding = Encoding.GetEncoding(28591);

            //for (var i = 0; i < 512; i++)
            //    Write($"{i} = {(char)i}\t");

            DisplayScreen();
            ReadLine();
        }

        private static void InitScreen()
        {
            var w1 = LargestWindowWidth - 2 - SepFieldStatusBoxesWitdh - StatusBoxesWitdh;
            var h1 = LargestWindowHeight - 2;

            CalcWindowSize(out int w, out int h);
            WindowS = new CoSize { w = w, h = h };
            SetWindowSize(w, h);
            BufferWidth = w;
            BufferHeight = h;
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

        static void DisplayPanelRobots()
        {
            //buff
        }
    }
}
