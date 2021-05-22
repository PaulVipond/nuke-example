using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface ICompileTarget
    {
        Target Compile => null;
    }
}
