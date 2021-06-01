using Nuke.Common;
using Nuke.Common.Execution;
using Fusion.Build.Tasks.TaskInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Fusion.Build.Haz;
using Fusion.Build.Tasks.DotNet;

namespace Fusion.Build
{
    public abstract class BaseBuild : NukeBuild, IHazConfiguration, IHazSolution, ICompile
    {
        CiStats ciStats;

        override protected void OnBuildCreated()
        {
            try
            {
                ciStats = new CiStats((this as IHazSolution).Solution.Name, (this as IHazConfiguration).Configuration);

                // TODO: Notification of build start (preferably via Teams one-to-one chat)
            }
            catch (Exception ex)
            {
                Logger.Warn($"WARNING: The build started successfully, but encountered an error while notifying the requesting user {ex}");
            }
        }

        override protected void OnBuildFinished()
        {
            try
            {
                ciStats.buildCompleted = DateTime.Now;
                ciStats.buildSucceeded = !IsFailing;
                ciStats.PopulateTargetOutcomesFromNukeTargets(SucceededTargets, ciStats.succeededTargets);
                ciStats.PopulateTargetOutcomesFromNukeTargets(FailedTargets, ciStats.failedTargets);
                ciStats.PopulateTargetOutcomesFromNukeTargets(AbortedTargets, ciStats.abortedTargets);
                ciStats.PopulateTargetOutcomesFromNukeTargets(SkippedTargets, ciStats.skippedTargets);

                // TODO: Register stats to accessible endpoint
            } 
            catch (Exception ex)
            {
                Logger.Warn($"WARNING: The build completed, but encountered an error while collecting and publishing final build stats {ex}");
            }

            try
            {
                // TODO: Notification of build completion (preferably via Teams one-to-one chat)
            }
            catch (Exception ex)
            {
                Logger.Warn($"WARNING: The build completed, but encountered an error while notifying the requesting user {ex}");
            }

            Logger.Info($"Build stats:\n{ciStats}");
        }
    }
}
