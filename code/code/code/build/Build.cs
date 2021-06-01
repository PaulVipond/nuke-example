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
using Fusion.Build;
using Fusion.Build.Tasks.DotNet;
using Fusion.Build.Haz;
using Fusion.Build.Haz.DotNet;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : BaseBuild, IClean, IRestore, IVersion, ICompile, IUnitTests, IPackage, IPrepareIntegrationEnvironment, IIntegrationTests, IPublish
{
    public static int Main() => Execute<Build>(x => (x as ICompile).Compile);
}
