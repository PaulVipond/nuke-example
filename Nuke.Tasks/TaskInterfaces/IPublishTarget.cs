using System;
using Nuke.Common;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface IPublishTarget
    {
        Target Publish => null;
    }
}
