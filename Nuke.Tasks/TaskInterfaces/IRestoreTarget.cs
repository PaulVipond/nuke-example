using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface IRestoreTarget
    {
        Target Restore => null;
    }
}
