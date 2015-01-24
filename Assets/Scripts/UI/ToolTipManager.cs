using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolTipManager : MonoBehaviour {
    public Text textObject;

    void Start() {
        gameObject.SetActive(false);
    }

    void Update() {
        textObject.transform.position = Input.mousePosition;
    }

    public void SetText(string text) {
        gameObject.SetActive(true);
        textObject.text = text;
    }

    public void ClearText() {
        gameObject.SetActive(false);
    }
}
