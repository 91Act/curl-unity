using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

using System.IO;
using System.Collections.Generic;
using System;

#if UNITY_IOS
#if UNITY_2019_3_OR_NEWER
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#else
using UnityEditor.iOS.Xcode.Custom;
using UnityEditor.iOS.Xcode.Custom.Extensions;
#endif
#endif

using System.Text.RegularExpressions;

public static class XCodePostProcessSDK
{
#if UNITY_EDITOR && UNITY_IOS
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        string targetGuid = "";

#if UNITY_2019_3_OR_NEWER
        targetGuid = pbxProject.GetUnityFrameworkTargetGuid();
#else
        targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");
#endif

        AddLibToProject(pbxProject, targetGuid, "libz.tbd");
        pbxProject.WriteToFile(projectPath);
    }

    static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
    {
        string fileGuid = inst.AddFile("/usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }
#endif
}
