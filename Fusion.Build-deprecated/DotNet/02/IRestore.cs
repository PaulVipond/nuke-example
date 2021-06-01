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
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

using Fusion.Build.Haz;
using Fusion.Build.Tasks.TaskInterfaces;

namespace Fusion.Build.Tasks.DotNet
{
    public interface IRestore: IHazSolution, IHazConfiguration, INukeBuild, IRestoreTarget
    {
        new Target Restore => _ => _
            .TryDependsOn<IClean>()
            .Executes(() =>
            {
                // Only restores 'Microsoft.VisualStudio.Azure.Fabric.MSBuild'
                NuGetRestore(restoreSettings => restoreSettings
                    .SetTargetPath(Solution)
                    .DisableProcessLogInvocation()
                    .DisableProcessLogOutput());

/*                MSBuild(settings => settings
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    // .SetFileVersion(BuildVersion)
                    .EnableRestore());
                // .DisableRestore());
*/            });
    }
}
