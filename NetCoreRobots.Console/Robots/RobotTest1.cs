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
using System.Threading.Tasks;
using NetCoreRobots.Core;
using NetCoreRobots.Sdk;

namespace NetCoreRobots.Console.Robots
{
    [RobotScriptClass]
    public class RobotTest1 : CSRobot
    {
        void main()
        {
            var a = 0;

            for (; ; )
            {
                if (a == 0)
                    a = rand(89);
                else if (a < 90)
                    a = 180 + rand(89);
                else if (a < 180)
                    a = 270 + rand(89);
                else if (a < 270)
                    a = rand(89);
                else
                    a = 90 + rand(89);
                drive(a, 100);
                while (speed() > 0) ;
            }
        }
        //async Task main()
        //{

        //}
    }
}
