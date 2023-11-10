#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEngine;

namespace ZaroDev.Tools
{
    public class BuildGenerator
    {
        static string GetProjectName(bool isRelease = false)
        {
            var buildType = isRelease ? "Release" : "Debug";
            return $"Builds/PawsAndClaws-{buildType}-{Application.version}.exe";
        }
        static BuildPlayerOptions GetCurrentBuildOptions(BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            return BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(defaultOptions);
        }

        [MenuItem("Build/Windows/Build Development + Tools")]
        public static void BuildDevtools()
        {
            BuildPlayerOptions buildPlayerOptions = GetCurrentBuildOptions();
            buildPlayerOptions.locationPathName = GetProjectName();
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.Development;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

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
            BuildPlayerOptions buildPlayerOptions = GetCurrentBuildOptions();
            buildPlayerOptions.locationPathName = GetProjectName();
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

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