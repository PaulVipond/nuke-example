using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using Fusion.Build.Tasks.TaskInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Fusion.Build.Haz;

namespace Fusion.Build.Tasks.DotNet
{
    public interface IClean : ICleanTarget, IHazArtifacts, IHazConfiguration, IHazSolution
    {
        new Target Clean => _ => _
            .Executes(() =>
            {
                EnsureCleanDirectory(ArtifactsDirectory);
                MSBuild(settings => settings
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .SetProcessArgumentConfigurator(args => args.Add("/t:clean"))
                    .DisableRestore());
            });
    }
}
