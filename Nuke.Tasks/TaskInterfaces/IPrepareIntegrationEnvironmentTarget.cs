using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface IPrepareIntegrationEnvironmentTarget
    {
        Target PrepareIntegrationEnvironment => null;
    }
}
