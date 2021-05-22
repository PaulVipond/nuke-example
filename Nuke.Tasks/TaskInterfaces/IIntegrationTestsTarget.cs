using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface IIntegrationTestsTarget
    {
        Target IntegrationTests => null;
    }
}
