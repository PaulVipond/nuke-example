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
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;

using Fusion.Build.Haz;
using Fusion.Build.Tasks.TaskInterfaces;

namespace Fusion.Build.Tasks.DotNet
{
    public interface ICompile: IHazSolution, IHazConfiguration, INukeBuild, ICompileTarget
    {
        new Target Compile => _ => _
            .TryAfter<IClean>()
            .TryAfter<IRestore>()
            .TryDependsOn<IVersion>()
            .Executes(() =>
            {
                MSBuild(settings => settings
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .DisableRestore());
            });
    }
}
