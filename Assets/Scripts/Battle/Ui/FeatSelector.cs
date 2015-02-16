using UnityEngine;
using System;

public class FeatSelector : MonoBehaviour {
    public delegate void Selector(Feat feat);

    private Feat[] _feats;
    private Selector _onSelect;
    private bool _shown;

    public void Show(Vector3 worldPos, Feat[] feats, Selector onSelect) {
        //_worldPos = worldPos; TODO: position in world relative to target
        _feats = feats;
        _onSelect = onSelect;
        _shown = true;
    }

    public void Hide() {
        _shown = false;
    }

    void OnGUI() {
        if (!_shown) { return; }

        GUILayout.BeginVertical("box");

        for (int i = 0; i < _feats.Length; i++) {
            if (GUILayout.Button(_feats[i].name)) {
                _onSelect(_feats[i]);
            }
        }

        GUILayout.EndVertical();
    }
}
