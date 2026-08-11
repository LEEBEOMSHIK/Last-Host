using UnityEditor;
using UnityEditor.SceneManagement;

namespace LastHost.Prototype.Cinematics.A01.Editor
{
    [InitializeOnLoad]
    public static class A01OfficePreviewLauncher
    {
        private const string Scene =
            "Assets/_Project/Scenes/Cinematics/Opening/A01OfficeAnimatic.unity";

        static A01OfficePreviewLauncher()
        {
            EditorApplication.playModeStateChanged += Changed;
            EditorApplication.delayCall += RestoreInterruptedPreviewOnDelayCall;
        }

        public static bool RestoreInterruptedPreview()
        {
            return !EditorApplication.isPlayingOrWillChangePlaymode &&
                   !EditorApplication.isPlaying &&
                   A01OfficePreviewSession.RestoreAndClear();
        }

        [MenuItem("Last Host/Cinematics/A01/Play Preview")]
        public static void PlayPreview()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            A01OfficePreviewSession.CaptureCurrent();
            try
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(Scene) == null)
                {
                    A01OfficeAnimaticSceneBuilder.RebuildPreview();
                }

                EditorSceneManager.OpenScene(Scene, OpenSceneMode.Single);
                EditorSceneManager.playModeStartScene = null;
                EditorApplication.EnterPlaymode();
            }
            catch
            {
                A01OfficePreviewSession.RestoreAndClear();
                throw;
            }
        }

        private static void RestoreInterruptedPreviewOnDelayCall()
        {
            RestoreInterruptedPreview();
        }

        private static void Changed(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode && A01OfficePreviewSession.HasSnapshot)
            {
                try
                {
                    EditorSceneManager.OpenScene(Scene, OpenSceneMode.Single);
                    EditorSceneManager.playModeStartScene = null;
                }
                catch
                {
                    A01OfficePreviewSession.RestoreAndClear();
                    throw;
                }
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorApplication.delayCall += RestoreInterruptedPreviewOnDelayCall;
            }
        }
    }
}
