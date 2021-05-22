Our environment has a very mixed code base - Powershell, Nuget packages, NPM packages, Javascript, Typescript, C#, Services etc. 
I want to make the build steps fairly rigid, in that most projects will repeat the same types of task, but allow different namespace
implementations for each task, e.g. Nuke.Tasks.DotNet, Nuke.Tasks.Git, Nuke.Tasks.Git, Nuke.Tasks.Powershell, Nuke.Tasks.Js

The generic build steps in each build would be taken care of by these interfaces which each define a target without implementation:

namespace Nuke.Tasks.TaskInterfaces

1.  ICaptureStats
2.  IRestoreTarget
3.  IVersionTarget
4.  ICompileTarget
5.  IUnitTestsTarget
6.  IPackageTarget
7.  IPrepareIntegrationEnvironmentTarget
8.  IIntegrationTestsTarget
9.  IPublishTarget
10. INotificationTarget
11. IPublishStatsTarget

Example:

    namespace Nuke.Tasks.TaskInterfaces
    
    public interface ICaptureStatsTarget
    {
        Target CaptureStats => null;
    }

Then you have:

    namespace Nuke.Tasks.DotNet
    
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

Note how the dependencies (.TryBefore...) are expressed with the implementation-less (generic) target. 
When an interface with implementation inherits from the generic interface, the target is named the same and 
overrides / hides it. This means if I really want to, I can take a compile task from Nuget.Tasks.DotNet and mix it with a 
publish task from Nuget.Tasks.Js. This is an extreme example, but there will definitely be times when I want to depend
on a version step that could be implemented in a number of different ways depending on the tech stack.

Everything compiles OK, but Nuke gives the following error at run-time:

Assertion failed: Property 'CaptureStats' must be implemented explicitly because it is inherited from multiple interfaces:
 - ICaptureStats
 - ICaptureStatsTarget
   at Nuke.Common.Utilities.ReflectionUtility.GetAllMembers(Type buildType, Func`2 filter, BindingFlags bindingFlags, Boolean allowAmbiguity)
   at Nuke.Common.Execution.ExecutableTargetFactory.GetTargetProperties(Type buildType)
   at Nuke.Common.Execution.ExecutableTargetFactory.CreateAll[T](T build, Expression`1[] defaultTargetExpressions)
   at Nuke.Common.Execution.BuildManager.Execute[T](Expression`1[] defaultTargetExpressions)