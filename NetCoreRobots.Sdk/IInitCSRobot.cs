using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreRobots.Sdk
{
    public interface IInitCSRobot
    {
        void Init(int argId, IArena argArena);
    }
}
