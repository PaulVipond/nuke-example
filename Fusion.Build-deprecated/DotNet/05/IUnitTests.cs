using System;
using System.Linq;
using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.DotCover;

using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.DotCover.DotCoverTasks;

using Fusion.Build.Haz;
using Fusion.Build.Tasks.TaskInterfaces;
using Nuke.Common.CI.TeamCity;

namespace Fusion.Build.Tasks.DotNet
{
    public interface IUnitTests: IHazSolution, IHazConfiguration, IHazArtifacts, INukeBuild, IUnitTestsTarget
    {
        new Target UnitTests => _ => _
            .TryAfter<IClean>()
            .TryAfter<IRestore>()
            .TryAfter<IVersion>()
            .TryDependsOn<ICompile>()
            .Executes(() =>
            {
                AbsolutePath testResultsDirectory = ArtifactsDirectory / "testResults";
                AbsolutePath coverageResults = testResultsDirectory / "DotCover";

                try
                {
                    IEnumerable<Project> allUnitTestsProject = Solution.GetProjects("*.Client*UnitTests");

                    Logger.Info($"Test results location: {testResultsDirectory}");
                    Logger.Info($"Coverage results location: {coverageResults}");
                    Logger.Info($"Discovered test projects:");
                    Array.ForEach(allUnitTestsProject.ToArray(), x => Logger.Info($"{x.Name}"));

                    DotCoverCoverDotNet(settings => settings
                       .When(IsLocalBuild, netSettings => netSettings
                           .SetOutputFile(coverageResults / "dotCover.html")
                           .SetReportType(DotCoverReportType.Html))
                       .When(!IsLocalBuild, netSettings => netSettings
                           .SetOutputFile(coverageResults / "dotCover.dcvr"))
                       .SetAnalyseTargetArguments(false)
                       .SetAttributeFilters("-:System.CodeDom.Compiler.GeneratedCodeAttribute")
                       .CombineWith(allUnitTestsProject, (settings, project) => settings
                           .SetProcessArgumentConfigurator(arguments => arguments
                               .Add($"-- test {project}")
                               .Add($"--configuration {Configuration}")
                               .Add("--no-restore")
                               .Add("--no-build")
                               .Add("--logger console;verbosity=normal")
                        // .Add("--logger trx")
                        )
                    ));
                }
                finally
                {
                    coverageResults.GlobFiles("*.dcvr").ForEach(coverageReport =>
                        TeamCity.Instance?.ImportData(TeamCityImportType.dotNetCoverage, coverageReport, TeamCityImportTool.dotcover));
                }
            });
    }
}
