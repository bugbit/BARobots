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

        static void Main(string[] args)
        {
            var x = LargestWindowWidth;

            SetWindowSize(x, 60);
            BufferWidth = x;
            BufferHeight = 60;
            //OutputEncoding = Encoding.GetEncoding(28591);

            //for (var i = 0; i < 512; i++)
            //    Write($"{i} = {(char)i}\t");

            DisplayScreen();
            ReadLine();
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
            var pWidth = BufferWidth - StatusBoxesWitdh - SepFieldStatusBoxesWitdh;
            var pHeigh = BufferHeight;
            var pW1 = pWidth - 1;
            var pH2 = pHeigh - 2;

            SetCursorPosition(0, 0);
            Write('┌');
            for (var x = 1; x < pW1; x++)
                Write('─');
            WriteLine('┐');
            for (var y = 1; y < pH2; y++)
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
    }
}
