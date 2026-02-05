using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TerrainScript))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GUILayoutOption[] glo = {GUILayout.Width(100), GUILayout.Height(34)};
        if (GUILayout.Button("Regenerate", glo))
        {
            TerrainScript ts = target as TerrainScript;
            ts.Regenerate();
        }
    }
}