using UnityEngine;
using System.Collections;

/// <summary>
/// Ensure that font is set for visibility on Linux
/// </summary>
public class SetGUIFont : MonoBehaviour {
    public Font font;
    void OnGUI() {
        GUI.skin.font = font;
    }
}
