#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;


public static class BuildScript
{
    public static string[] GetBuildSceneList()
    {
        var scenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        return scenes.ToArray();
    }

    [MenuItem("Build/android")]
    public static void Android()
    {
        var scenes = GetBuildSceneList();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.buildAppBundle = false;

        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/android/build.apk",
            target = BuildTarget.Android,
            options = BuildOptions.Development,
        });
    }

    public static void AndroidIL2CPP()
    {
        var scenes = GetBuildSceneList();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        EditorUserBuildSettings.buildAppBundle = false;

        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/android_il2cpp/build.apk",
            target = BuildTarget.Android,
            options = BuildOptions.Development,
        });
    }

    public static void AndroidIL2CPPStore()
    {
        var scenes = GetBuildSceneList();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        EditorUserBuildSettings.buildAppBundle = true;

        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/android_il2cpp/build.aab",
            target = BuildTarget.Android,
            options = BuildOptions.None,
        });
    }

    [MenuItem("Build/ios")]
    public static void iOS()
    {
        var scenes = GetBuildSceneList();
        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/ios",
            target = BuildTarget.iOS,
            options = BuildOptions.Development | BuildOptions.AllowDebugging,
        });
    }

    [MenuItem("Build/macos")]
    public static void MacOS()
    {
        var scenes = GetBuildSceneList();
        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/macos/curl-unity.app",
            target = BuildTarget.StandaloneOSX,
            options = BuildOptions.Development | BuildOptions.AllowDebugging,
        });
    }

    [MenuItem("Build/linux")]
    public static void Linux()
    {
        var scenes = GetBuildSceneList();
        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/linux/curl-unity.x86_64",
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.Development | BuildOptions.AllowDebugging,
        });
    }

    [MenuItem("Build/windows")]
    public static void Windows()
    {
        var scenes = GetBuildSceneList();
        build(new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = "out/windows/curl-unity",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.Development | BuildOptions.AllowDebugging,
        });
    }


    [MenuItem("Build/sync")]
    public static void Sync()
    {
        Packages.Rider.Editor.RiderScriptEditor.SyncSolution();
        maybeExit(0);
    }

    private static void build(BuildPlayerOptions options)
    {
        var report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log($"Build ({options.target}) succeeded.");
            maybeExit(0);
        }
        else
        {
            UnityEngine.Debug.LogError($"Build for [{options.target}]: {report.summary.result}...");
            maybeExit(1);
        }
    }

    private static void maybeExit(int exitcode)
    {
        Debug.Log($"maybeExit isBatchMode={Application.isBatchMode}, exitcode={exitcode}");
        if (Application.isBatchMode)
        {
            EditorApplication.Exit(exitcode);
        }
    }
}
#endif
