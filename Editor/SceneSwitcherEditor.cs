using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.SceneManagement;
using System.Linq;

namespace GameBrewStudios.Tools
{
    public class SceneSwitcher : EditorWindow
    {
        [MenuItem("Window/GameBrew Studios/Tools/Scene Switcher")]
        static void Init()
        {
            SceneSwitcher window = (SceneSwitcher)EditorWindow.GetWindow(typeof(SceneSwitcher));

            window.Show();
        }

        static string path = "";

        FileInfo[] files;

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            if (string.IsNullOrEmpty(path))
            {
                path = EditorPrefs.GetString("SceneSwitcher Path", Application.dataPath);
                RefreshSceneList();
            }

            if(GUILayout.Button("Select Folder"))
            {
                path = EditorUtility.OpenFolderPanel("Select a Scenes Folder", Application.dataPath, "Scenes");
                EditorPrefs.SetString("SceneSwitcher Path", path);
                RefreshSceneList();
            }

            if(GUILayout.Button("Refresh List"))
            {
                RefreshSceneList();
            }

            EditorGUILayout.Space();

            if (files != null && files.Length > 0)
            {
                foreach (FileInfo fi in files)
                {
                    if (fi.Name.EndsWith(".unity"))
                    {
                        if (GUILayout.Button(fi.Name.Replace(".unity", "")))
                        {
                            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                            EditorSceneManager.OpenScene(fi.FullName, OpenSceneMode.Single);

                        }
                    }
                }
            }
            

            if(files == null || files.Length < 1)
            {
                GUILayout.Label("No scenes found.");
            }
            
            EditorGUILayout.EndVertical();
        }

        private void RefreshSceneList()
        {
            DirectoryInfo di = new DirectoryInfo(path);
            files = di.GetFiles().Where(x => x.Name.Contains(".unity")).ToArray();
        }
    }
}
