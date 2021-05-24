using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Tasks.TaskInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuke.Tasks.Build
{
    public abstract class BaseBuild : NukeBuild
    {
        // Provide a bootstrap step that executes before a standard build
        protected Target Bootstrap => _ => _
            .TryBefore<ICleanTarget>()
            .TryBefore<IRestoreTarget>()
            .TryBefore<IVersionTarget>()
            .TryBefore<ICompileTarget>()
            .TryBefore<IUnitTestsTarget>()
            .TryBefore<IPackageTarget>()
            .TryBefore<IPrepareIntegrationEnvironmentTarget>()
            .TryBefore<IIntegrationTestsTarget>()
            .TryBefore<IPublishTarget>()
            .Executes(() =>
            {
                Logger.Info("Starting build...");
            });

        CiStats ciStats = new CiStats();

        override protected void OnBuildFinished()
        {
            ciStats.buildCompleted = DateTime.Now;
            ciStats.buildSucceeded = !IsFailing;
            ciStats.PopulateTargetOutcomesFromNukeTargets(SucceededTargets, ciStats.succeededTargets);
            ciStats.PopulateTargetOutcomesFromNukeTargets(FailedTargets, ciStats.failedTargets);
            ciStats.PopulateTargetOutcomesFromNukeTargets(AbortedTargets, ciStats.abortedTargets);
            ciStats.PopulateTargetOutcomesFromNukeTargets(SkippedTargets, ciStats.skippedTargets);

            // TODO: Register stats to accessible endpoint

            Logger.Info($"Build {(IsFailing ? "FAILED" : "SUCCEEDED")}, stats:\n{ciStats}");
        }
    }
}
