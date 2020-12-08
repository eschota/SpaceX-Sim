using UnityEngine;
using UnityEditor;

public class CustomWindow : EditorWindow
{
    private Texture _texture;

    [MenuItem("Window/GetColorsFromTexture")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CustomWindow window = (CustomWindow)EditorWindow.GetWindow(typeof(CustomWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("This window for set Colors", EditorStyles.boldLabel);
        if (GUILayout.Button("SetColorsEarth"))
        {
            SetColorsEarth.SetColors();
        }
    }
}