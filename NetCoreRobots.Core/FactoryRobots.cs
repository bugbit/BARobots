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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetCoreRobots.Core.Robots;
using NetCoreRobots.Sdk;

namespace NetCoreRobots.Core
{
    public class FactoryRobots
    {
        private Dictionary<string, Type> mCache = new Dictionary<string, Type>();
        private Dictionary<string, Type> mClassType = new Dictionary<string, Type>();

        public FactoryRobots()
        {
            AddRobotClassAssemblty(typeof(RobotScriptClassAttribute).Assembly);
        }

        public void AddRobotClassAssemblty(Assembly argAssembly)
        {
            var pQuery =
                from t in argAssembly.GetTypes()
                where t.IsAssignableFrom(typeof(CSRobot))
                let a = t.GetCustomAttribute<RobotScriptClassAttribute>()
                where a != null
                select new { t.Name, t };

            foreach (var t in pQuery)
                mClassType[t.Name] = t.t;
        }

        public (CSRobot, Func<Task>) Create(string argName)
        {
            if (!mCache.TryGetValue(argName, out Type pType))
            {
                if (!mClassType.TryGetValue(argName, out pType))
                {

                }
            }

            if (pType == null)
                throw new ApplicationException(string.Format(Properties.Resources.NoRobotFound, argName));

            var pMainMethods = pType.FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, (f, c) => f.Name.Equals((string)c), "main").OfType<MethodInfo>().ToArray();

            if (pMainMethods.Length == 0)
                throw new ApplicationException(string.Format(Properties.Resources.NoMainMethodFound, argName));

            if (pMainMethods.Length != 1)
                throw new ApplicationException(string.Format(Properties.Resources.More1MainMethod, argName));

            var pMainMethod = pMainMethods[0];

            if (!(pMainMethod.GetParameters().Length == 0 && (pMainMethod.ReturnType == typeof(void) || pMainMethod.ReturnType == typeof(Task))))
                throw new ApplicationException(string.Format(Properties.Resources.MainMethodIncorrect, argName));

            var pMain =
                (pMainMethod.ReturnType == typeof(void))
                    ? () => Task.Run((Action)Delegate.CreateDelegate(typeof(Action), pMainMethod))
                    : (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), pMainMethod);
            var pRobot = Activator.CreateInstance(pType);


            return ((CSRobot)pRobot, pMain);
        }
    }
}
