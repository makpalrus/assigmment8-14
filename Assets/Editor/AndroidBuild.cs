using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class AndroidBuild
{
    [MenuItem("Skybound/Build Android APK")]
    public static void Build()
    {
        EditorUserBuildSettings.buildAppBundle = false;
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
            locationPathName = "output/Skybound.apk",
            target = BuildTarget.Android
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException($"Android build ended with {report.summary.result}.");
    }
}
