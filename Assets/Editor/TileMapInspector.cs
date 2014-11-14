using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileMapMesh))]
public class TileMapEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("regenerate")) {
            var tilemap = (TileMapMesh)target;
            tilemap.BuildMesh();
        }
    }
}
