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
using Nuke.Tasks.Build;
using Nuke.Tasks.DotNet;
using Nuke.Tasks.TaskInterfaces;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild, ICaptureStats, IClean, ICompile
{
    public static int Main () => Execute<Build>(x => x.Bootstrap);

    Target Bootstrap => _ => _
        .TryBefore<ICaptureStatsTarget>()
        .Executes(() =>
        {
            Logger.Info("Starting build...");
        });
}