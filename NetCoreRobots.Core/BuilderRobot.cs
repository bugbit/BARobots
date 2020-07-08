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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreRobots.Core
{
    public class BuilderRobot
    {
        //protected Type mRobotClassType;

        //public virtual void Compile() { }

        //public virtual Robot Create()
        //{
        //    Compile();

        //    if (mRobotClassType == null)
        //        throw new NullReferenceException($"{nameof(mRobotClassType)} is null");

        //    var pMethod = mRobotClassType.GetMethod("main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //    if (pMethod == null)
        //        throw new InvalidOperationException($"{mRobotClassType.Name} no define main method");

        //    if (pMethod.GetParameters().Length == 0)
        //    {
        //        if (pMethod.DeclaringType == typeof(void))
        //            return new Robot(mRobotClassType.Name, (Action)Action.CreateDelegate(typeof(Action), pMethod));
        //        if (pMethod.DeclaringType == typeof(Task))
        //            return new Robot(mRobotClassType.Name, (Func<Task>)Func<Task>.CreateDelegate(typeof(Func<Task>), pMethod));
        //    }

        //    throw new InvalidCastException($"{pMethod} no define method void main() or Task main()");
        //}
    }
}
