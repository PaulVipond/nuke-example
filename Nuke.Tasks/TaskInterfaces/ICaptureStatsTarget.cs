using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface ICaptureStatsTarget
    {
        Target CaptureStats => null;
    }
}
