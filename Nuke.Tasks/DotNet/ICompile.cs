using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

using Nuke.Tasks.Haz;
using Nuke.Tasks.TaskInterfaces;

namespace Nuke.Tasks.DotNet
{
    public interface ICompile: IHazSolution, IHazConfiguration, INukeBuild, ICompileTarget
    {
        new Target Compile => _ => _
            .TryBefore<IUnitTestsTarget>()
            .TryBefore<IPackageTarget>()
            .TryBefore<IPrepareIntegrationEnvironmentTarget>()
            .TryBefore<IIntegrationTestsTarget>()
            .TryBefore<IPublishTarget>()
            .TryBefore<INotificationTarget>()
            .TryBefore<IPublishStatsTarget>()
            .Executes(() =>
            {
                DotNetBuild(s => s
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            });
    }
}
