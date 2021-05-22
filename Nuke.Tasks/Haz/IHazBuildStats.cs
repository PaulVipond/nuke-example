using System;
using System.Collections.Generic;
using System.Text;

namespace Nuke.Tasks.Haz
{
    public interface IHazBuildStats
    {
        CiStats CiStats => new CiStats();
    }
}
