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

using Fusion.Build.Haz;
using Fusion.Build.Tasks.TaskInterfaces;

namespace Fusion.Build.Tasks.DotNet
{
    public interface IPublish: IHazSolution, IHazConfiguration, INukeBuild, IPublishTarget
    {
        new Target Publish => _ => _
            .TryAfter<IClean>()
            .TryAfter<IRestore>()
            .TryAfter<IVersion>()
            .TryAfter<ICompile>()
            .TryAfter<IUnitTests>()
            .TryAfter<IPackage>()
            .TryAfter<IPrepareIntegrationEnvironment>()
            .TryDependsOn<IIntegrationTests>()
            .Executes(() =>
            {
            });
    }
}
