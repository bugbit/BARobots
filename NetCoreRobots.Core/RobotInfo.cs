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
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetCoreRobots.Sdk;

using static System.Math;
using static NetCoreRobots.Core.MathEx;

namespace NetCoreRobots.Core
{
    public sealed class RobotInfo
    {
        private double mAngle;

        public IInitCSRobot CSRobot { get; set; }
        public int IdRobot { get; set; }
        public string Name { get; set; }
        public int IdTeamOrRobot { get; set; }
        public int? IdTeam { get; set; }
        public int? IdMemberTeam { get; set; }
        public Func<Task> Main { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
        public int PorSpeed { get; set; }
        public double Speed { get; set; }  // m/s
        public double SpeedTo { get; set; }  // m/s
        public double Angle
        {
            get => mAngle;
            set
            {
                var pRad = Deg2Rad(value);

                mAngle = value;
                AngCos = Cos(pRad);
                AngSin = Sin(pRad);
            }
        }   // en rad
        public double LocX { get; set; }    // m
        public double LocY { get; set; }    // m
        public double AngCos { get; private set; }
        public double AngSin { get; private set; }

        //public RobotInfo(string argName, Func<Task> argMain)
        //{
        //    Name = argName;
        //    mMain = argMain;
        //}

        //public RobotInfo(string argName, Action argMain) : this(argName, () => Task.Run(argMain)) { }
    }
}
