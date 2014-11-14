using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("regenerate")) {
            var tilemap = (TileMap)target;
            tilemap.BuildMesh();
        }
    }
}
