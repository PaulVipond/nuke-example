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
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;

using Fusion.Build.Haz;
using Fusion.Build.Tasks.TaskInterfaces;

namespace Fusion.Build.Tasks.DotNet
{
    public interface IPackage: IHazSolution, IHazConfiguration, IHazPackageArtifacts, INukeBuild, IPackageTarget
    {
        new Target Package => _ => _
            .TryAfter<IClean>()
            .TryAfter<IRestore>()
            .TryAfter<IVersion>()
            .TryAfter<ICompile>()
            .TryDependsOn<IUnitTests>()
            .Produces(PackageArtifactsDirectory / "*.nupkg")
            .Executes(() =>
            {
                NuGetPack(settings => settings
                    .SetConfiguration(Configuration)
                    //.SetVersion(BuildVersion)
                    .SetOutputDirectory(PackageArtifactsDirectory)
                    .CombineWith(Solution.Directory.GlobFiles("*.Client.*.nuspec"), (settings, nuspec) => settings
                        .SetTargetPath(nuspec)
                        .SetBasePath(GetPackageBasePath(nuspec))));

                string GetPackageBasePath(AbsolutePath path)
                {
                    string packageName = path.ToString()
                        .Split('/', '\\')
                        .Last()
                        .Replace(".nuspec", "");

                    // TODO Get rid of 'Api' in the project name?
                    if (packageName.Contains(".Client.Grpc") || packageName.Contains(".Client.Rest"))
                    {
                        packageName = packageName.Replace(".Client.", ".Client.Api.");
                    }

                    string binariesDirectory = packageName.Contains("PowerShell") ? "pkg" : "bin";

                    return Solution.Directory / packageName / binariesDirectory / Configuration;
                }

            });
    }
}
