using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface IUnitTestsTarget
    {
        Target UnitTests => null;
    }
}
