#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoFocusGameView
{
    static AutoFocusGameView()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                FocusGameView();
            }
        };
    }

    private static void FocusGameView()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Game");
    }
}
#endif