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
using NetCoreRobots.Core;

namespace NetCoreRobots.Console
{
    class CoRobotInfo
    {
        public RobotInfo RobotInfo { get; set; }
        public CoPos PosAnt { get; set; }
        public CoPos Pos { get; } = new CoPos();

        public void SetPos(int x, int y) => Pos.SetPos(x, y);

        public void ActPos()
        {
            if (PosAnt == null)
                PosAnt = new CoPos { x = Pos.x, y = Pos.y };
            else
            {
                PosAnt.x = Pos.x;
                PosAnt.y = Pos.y;
            }
        }
    }
}
