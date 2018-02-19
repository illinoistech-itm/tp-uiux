// InstallOptions_FluvioExamplesThree.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ASInstaller = ASInstaller_FluvioExamplesThree;

#if !ASINSTALLER_DEVELOPMENT
[InitializeOnLoad]
#endif

public static class InstallOptions_FluvioExamplesThree
{
    static bool s_OpenExampleScene;

    #region Constructors
    static InstallOptions_FluvioExamplesThree()
    {
        ASInstaller.OnInstallGUI += OnInstallGUI;
        ASInstaller.OnPostInstall += OnPostInstall;
    }
    #endregion

    static bool OnInstallGUI()
    {
        var versionInfoType = AppDomain.CurrentDomain.GetAssemblies().TrySelectMany(assembly => assembly.GetTypes()).FirstOrDefault(type => type.FullName == "Thinksquirrel.Fluvio.Internal.VersionInfo");
        var fluvioInstalled = versionInfoType != null;

        if (!fluvioInstalled)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("This example package requires");
            GUILayout.Space(-4f);
            GUI.color = new Color(0, .5f, .75f, 1);
            if (GUILayout.Button("Fluvio", EditorStyles.whiteLabel))
                Application.OpenURL("com.unity3d.kharma:content/2888");
            GUI.color = Color.white;
            GUILayout.Space(-4f);
            GUILayout.Label("in order to be installed.");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else
        {
            s_OpenExampleScene = GUILayout.Toggle(s_OpenExampleScene, "Open Example Scene");
        }

        return fluvioInstalled;
    }
    static bool OnPostInstall(string packageName)
    {
        if (s_OpenExampleScene)
        {
            EditorPrefs.SetString("Thinksquirrel.FluvioEditor.OpenExampleScene", "Three");
            EditorPrefs.SetString("ASInstaller.LastInstall.Options", "OpenExampleScene");
        }

        return true;
    }
    static IEnumerable<TResult> TrySelectMany<TSource, TResult>(this IEnumerable<TSource> seq, Func<TSource, IEnumerable<TResult>> selector)
    {
        return seq.SelectMany(i =>
        {
            try
            {
                return selector(i);
            }
            catch (Exception)
            {
                return Enumerable.Empty<TResult>();
            }
        });
    }
}
