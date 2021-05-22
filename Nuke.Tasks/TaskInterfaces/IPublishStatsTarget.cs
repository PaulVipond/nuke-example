using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface IPublishStatsTarget
    {
        Target PublishStats => null;
    }
}
