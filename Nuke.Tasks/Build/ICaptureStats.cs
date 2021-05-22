using Nuke.Common;
using Nuke.Tasks.TaskInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Nuke.Tasks.Haz;

namespace Nuke.Tasks.Build
{
    public interface ICaptureStats : ICaptureStatsTarget, IHazBuildStats
    {

        new Target CaptureStats => _ => _
            .TryBefore<ICleanTarget>()
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
                var jsonString = JsonSerializer.Serialize<CiStats>(CiStats);
                Logger.Info($"Stats captured:\n{jsonString}");
            });
    }
}
