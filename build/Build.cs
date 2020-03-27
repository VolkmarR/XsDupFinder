using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions(
    "publish", 
    GitHubActionsImage.WindowsLatest, 
    On = new GitHubActionsTrigger[] { GitHubActionsTrigger.PullRequest },
    InvokedTargets = new string[]{ nameof(Publish) }
    )]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PublishFileName => ArtifactsDirectory / "XsDupFinder.zip";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Restore"));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Rebuild")
                .SetConfiguration(Configuration)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetVerbosity(MSBuildVerbosity.Minimal)
                .SetNodeReuse(IsLocalBuild));
        });

    Target Publish => _ => _
        .Produces(PublishFileName)
        .DependsOn(Clean)
        .DependsOn(Compile)
        .Executes(() =>
        {
            var OutputDirectory = ArtifactsDirectory / "XsDupeFinder";

            MSBuild(s => s
                .SetTargetPath(Solution.GetProject("XsDupFinderCmd"))
                .SetTargets("Build")
                .SetOutDir(OutputDirectory)
                .SetConfiguration(Configuration)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetVerbosity(MSBuildVerbosity.Quiet)
                .SetNodeReuse(IsLocalBuild));

            MSBuild(s => s
                .SetTargetPath(Solution.GetProject("XsDupFinderWin"))
                .SetTargets("Build")
                .SetOutDir(OutputDirectory)
                .SetConfiguration(Configuration)
                .SetVerbosity(MSBuildVerbosity.Quiet)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetNodeReuse(IsLocalBuild));

            CopyFileToDirectory(SourceDirectory / ".." / "readme.md", OutputDirectory);
            CopyFileToDirectory(SourceDirectory / ".." / "LICENSE", OutputDirectory);

            Compress(OutputDirectory, PublishFileName);
        });
}
