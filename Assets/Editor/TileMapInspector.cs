using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("regenerate")) {
            var tilemap = (TileMap)target;
            tilemap.GenerateMap();
        }
    }

    void OnSceneGUI() {
        Event ev = Event.current;
        if (ev.type == EventType.MouseUp) {
            var mesh = ((TileMap)target).GetComponent<TileMapMesh>();
            Ray ray = Camera.current.ScreenPointToRay(ev.mousePosition);

        }
    }
}
