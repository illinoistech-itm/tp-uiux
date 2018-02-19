// ASInstallerWindow_FluvioExamplesThree.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.

using ASInstaller = ASInstaller_FluvioExamplesThree;
using ASInstallerWindow = ASInstallerWindow_FluvioExamplesThree;
using UnityEditor;
using UnityEngine;

public class ASInstallerWindow_FluvioExamplesThree : EditorWindow
{
    #region Instance Fields
    Texture2D m_KeyImage;
    Rect m_KeyImageRect = new Rect(4, 4, 512, 64);
    Rect m_MainAreaRect = new Rect(4, 72, 512, 324);
    TextAsset m_Readme;
    Vector2 m_ReadmeScrollPosition;
    bool m_ViewingReadme;
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        m_KeyImage = Resources.Load("ASInstaller_FluvioExamplesThree-512x64", typeof (Texture2D)) as Texture2D;
        m_Readme = Resources.Load("README_FluvioExamplesThree", typeof (TextAsset)) as TextAsset;
        minSize = new Vector2(520, 400);
        maxSize = new Vector2(520, 400);
        position = new Rect(position.x, position.y, 520, 400);
    }
    void OnGUI()
    {
        GUI.DrawTexture(m_KeyImageRect, m_KeyImage);

        GUILayout.BeginArea(m_MainAreaRect, GUI.skin.box);

        if (m_ViewingReadme)
        {
            m_ReadmeScrollPosition = GUILayout.BeginScrollView(m_ReadmeScrollPosition, false, false,
                                                               GUILayout.Width(502), GUILayout.Height(292));

            GUILayout.Label(m_Readme.text, EditorStyles.wordWrappedLabel);

            GUILayout.EndScrollView();

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Done", GUILayout.Height(22)))
                m_ViewingReadme = false;

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.FlexibleSpace();

            // Description
            GUILayout.Label(ASInstaller._description, EditorStyles.wordWrappedLabel);

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            // Custom GUI
            var canInstall = ASInstaller.OnInstallGUI_Invoke();

            // Install
            EditorGUI.BeginDisabledGroup(!canInstall);
            if (GUILayout.Button(string.Format("Install {0}", ASInstaller._displayName), GUILayout.Height(30)))
            {
                ASInstaller.StartInstall();
                Close();
            }
            EditorGUI.EndDisabledGroup();

            // View readme
            if (m_Readme)
            {
                if (GUILayout.Button("View README", GUILayout.Height(30)))
                    m_ViewingReadme = true;
            }
            GUILayout.FlexibleSpace();

            // Installer version information
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(ASInstaller._installerVersionString, EditorStyles.miniLabel);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        GUILayout.EndArea();
    }
    #endregion
}
