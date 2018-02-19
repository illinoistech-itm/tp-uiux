// ASInstaller_FluvioExamplesThree.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.

using ASInstaller = ASInstaller_FluvioExamplesThree;
using ASInstallerWindow = ASInstallerWindow_FluvioExamplesThree;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#pragma warning disable 162
#if !ASINSTALLER_DEVELOPMENT
[InitializeOnLoad]
#endif

public static class ASInstaller_FluvioExamplesThree
{
    #region Settings - Edit these
    // The display name for the package, shown to the user.
    internal static string _displayName = "Fluvio Examples: Three";

    // A short description for interactive installation.
    internal static string _description =
        string.Format(
            "This wizard will install Fluvio Examples: Three for your Unity version into your project. Please select one of the following options to continue.\n\nIf you exit this installer early, you can re-run it by going to {0}.",
            _menuItem.Replace("/", " > "));

    // Menu item, in case the installation window is closed.
    internal const string _menuItem = "Window/Fluvio/Install Fluvio Examples: Three";
    internal const int _menuItemPosition = 10000;

    // The base folder under which all .zip files should be placed
    // THIS FOLDER SHOULD BE UNIQUE, and not the same folder that the package is installed into!
    const string _baseFolder = "ASInstaller_FluvioExamplesThree";

    // If enabled, install proceeds silently without any user intervention needed.
    const bool s_SilentInstall = false;

    // If enabled, installation files will be removed. This will remove the entire base folder.
    // It is highly recommended to keep this enabled, or you will HAVE to handle cleanup manually!
    // Disabling this without a proper cleanup procedure will cause the installation to repeat every time a script is compiled.
    const bool s_DeleteInstallationFiles = true;

    // The name of the package (without extension).
    const string _packageName = "FluvioExamplesThree";

    // The name of the package folder.
    const string _packageFolder = "Fluvio Examples";
    #endregion

    #region Callbacks
    // This callback runs before installation. Feel free to subscribe to this event from any editor script.
    // param: string - the name of the package that will be installed
    // returns: bool - whether or not the action was successful. If false, installation will abort.
    public static event Func<string, bool> OnPreInstall;

    // This callback runs after installation, but before any file deletion. Feel free to subscribe to this event from any editor script.
    // param: string - the name of the package that was installed
    // returns: bool - whether or not the action was successful. If false, installation will abort.
    public static event Func<string, bool> OnPostInstall;

    // This callback runs in the GUI, between the description text and buttons. Use this to insert your own GUI elements.
    // Note that there is limited space to put elements.
    // returns: bool - whether or not installation is possible. If false, the install button will be disabled.
    public static event Func<bool> OnInstallGUI;
    #endregion

    #region Internal stuff
    internal const string _installerVersionString = "ASInstaller v. 1.4.0";

    // Runs on compile, immediately after installing from the Asset Store
    static ASInstaller_FluvioExamplesThree()
    {
        if (EditorPrefs.GetBool(_packageName + ".DoFinalInstallerStep", false))
        {
            EditorApplication.update += DoFinalInstallerStep;
        }
        else if (s_SilentInstall)
        {
            EditorApplication.update += StartInstallDelayed;
        }
        else
        {
            EditorApplication.update += GetWindowDelayed;
        }
    }
    static void StartInstallDelayed()
    {
        if (s_CurrentFrame++ > 10)
        {
            s_CurrentFrame = 0;
            EditorApplication.update -= StartInstallDelayed;
            StartInstall();
        }
    }
#if !ASINSTALLER_DEVELOPMENT
    [MenuItem(_menuItem, false, _menuItemPosition)]
#endif
    static void GetWindow()
    {
        // Open the editor window for interactive installation
        EditorWindow.GetWindow<ASInstallerWindow>(true, string.Format("{0} Installer", _displayName), true);
    }
    static int s_CurrentFrame;
    static void GetWindowDelayed()
    {
        if (s_CurrentFrame++ > 10)
        {
            s_CurrentFrame = 0;
            EditorApplication.update -= GetWindowDelayed;
            GetWindow();
        }
    }
    internal static bool OnInstallGUI_Invoke()
    {
        return OnInstallGUI == null || OnInstallGUI();
    }
    internal static void StartInstall()
    {
        // OnPreInstall
        if (OnPreInstall != null)
        {
            if (!OnPreInstall(_packageName))
            {
                Debug.LogError(string.Format("Unable to install {0}! Error: OnPreInstall failure", _packageName));
                return;
            }
        }

        try
        {
            // Extract file
            var di = new DirectoryInfo(Application.dataPath);
            var p = Path.Combine(di.FullName, Path.Combine(_baseFolder, _packageName + ".unitypackage"));

            AssetDatabase.ImportPackage(p, false);
        }
        catch
        {
            Debug.LogError(string.Format("Unable to install {0}! Error: Package import failure", _packageName));
            return;
        }

        // OnPostInstall
        if (OnPostInstall != null)
        {
            if (!OnPostInstall(_packageName))
            {
                Debug.LogError(string.Format("Unable to install {0}! Error: OnPostInstall failure", _packageName));
                return;
            }
        }

        EditorPrefs.SetString("ASInstaller.LastInstall.PackageName", _packageName);
        EditorPrefs.SetString("ASInstaller.LastInstall.DisplayName", _displayName);
        EditorPrefs.SetString("ASInstaller.LastInstall.InstallerVersion", _installerVersionString);
        EditorPrefs.SetBool(_packageName + ".DoFinalInstallerStep", true);
    }
    static void DoFinalInstallerStep()
    {
        EditorApplication.update -= DoFinalInstallerStep;

        EditorPrefs.SetBool(_packageName + ".DoFinalInstallerStep", false);

        // Clear log
        ClearLog();

        // Reimport
        AssetDatabase.ImportAsset(string.Format("Assets/{0}", _packageFolder), ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);

        // Delete base folder
        if (s_DeleteInstallationFiles)
            DeleteBaseFolder();
    }
    static void ClearLog()
    {
        // This simply does "LogEntries.Clear()" the long way:
#if UNITY_2017_1_OR_NEWER
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
#else
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
#endif
        if (logEntries != null)
        {
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
    }
    static void DeleteBaseFolder()
    {
        // Some sanity checks
        if (string.IsNullOrEmpty(_baseFolder) || _baseFolder.StartsWith("/", StringComparison.Ordinal) ||
            _baseFolder.StartsWith("\\", StringComparison.Ordinal))
            return;

        FileUtil.DeleteFileOrDirectory(string.Format("Assets/{0}", _baseFolder));
        if (File.Exists(string.Format("Assets/{0}.meta", _baseFolder)))
        {
            FileUtil.DeleteFileOrDirectory(string.Format("Assets/{0}.meta", _baseFolder));
        }

        AssetDatabase.Refresh();
    }
    #endregion
}
