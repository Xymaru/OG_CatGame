#if UNITY_EDITOR

using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEngine;

namespace ZaroDev.Tools
{
    public static class BuildGenerator
    {
        private static readonly BuildOptionsSO BuildOptions = Resources.Load<BuildOptionsSO>("BuildOptions");

        static string GetProjectName(bool isRelease = false)
        {
            var buildType = isRelease ? "Release" : "Debug";
            return $"{buildType}/{BuildOptions.BuildName}-{buildType}-{Application.version}.exe";
        }
        static string GetProjectFullPath(bool isRelease = false)
        {
            return $"{BuildOptions.BuildPath}/{GetProjectName(isRelease)}";
        }

        static BuildPlayerOptions GetCurrentBuildOptions(BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            return BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultOptions);
        }

        [MenuItem("Build/Windows/Build Development + Tools")]
        public static void BuildDevtools()
        {
            var buildPlayerOptions = GetCurrentBuildOptions();
            buildPlayerOptions.locationPathName = GetProjectFullPath();
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = UnityEditor.BuildOptions.Development;

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        [MenuItem("Build/Windows/Build Release")]
        public static void BuildRelease()
        {
            var buildPlayerOptions = GetCurrentBuildOptions();
            buildPlayerOptions.locationPathName = GetProjectFullPath();
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
    }
}
#endif