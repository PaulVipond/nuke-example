using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface ICleanTarget
    {
        Target Clean => null;
    }
}
