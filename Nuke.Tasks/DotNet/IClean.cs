using Nuke.Common;
using static Nuke.Common.IO.FileSystemTasks;
using Nuke.Tasks.TaskInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Nuke.Tasks.Haz;

namespace Nuke.Tasks.DotNet
{
    public interface IClean : ICleanTarget, IHazArtifacts
    {
        new Target Clean => _ => _
            .TryBefore<IRestoreTarget>()
            .TryBefore<IVersionTarget>()
            .TryBefore<ICompileTarget>()
            .TryBefore<IUnitTestsTarget>()
            .TryBefore<IPackageTarget>()
            .TryBefore<IPrepareIntegrationEnvironmentTarget>()
            .TryBefore<IIntegrationTestsTarget>()
            .TryBefore<IPublishTarget>()
            .TryBefore<INotificationTarget>()
            .TryBefore<IPublishStatsTarget>()
            .Executes(() =>
            {
                EnsureCleanDirectory(ArtifactsDirectory);
            });
    }
}
