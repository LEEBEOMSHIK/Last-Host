using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LastHost.Prototype.Editor.Startup
{
    [InitializeOnLoad]
    public static class StartupPlayModeBootstrap
    {
        public const string StartupScenePath = "Assets/_Project/Scenes/Startup.unity";
        public const string MissingStartupSceneDiagnosticId = "[StartupPlay:PFC1_MISSING_START_SCENE]";

        static StartupPlayModeBootstrap()
        {
            ConfigurePlayModeStartScene();
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            ConfigurePlayModeStartScene();
        }

        public static bool ConfigurePlayModeStartScene()
        {
            var startupScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartupScenePath);
            if (startupScene == null)
            {
                EditorSceneManager.playModeStartScene = null;
                Debug.LogError(
                    $"{MissingStartupSceneDiagnosticId} Required saved scene was not found at '{StartupScenePath}'.");
                return false;
            }

            if (EditorSceneManager.playModeStartScene != startupScene)
            {
                EditorSceneManager.playModeStartScene = startupScene;
            }

            return true;
        }
    }
}
