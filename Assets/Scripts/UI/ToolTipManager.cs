using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolTipManager : MonoBehaviour {
    public Text textObject;
    private bool _textSet, _textCleared;

    void Start() {
        gameObject.SetActive(false);
    }

    void Update() {
        textObject.transform.position = Input.mousePosition;
        if (_textSet) {
            gameObject.SetActive(true);
        }
        else if (_textCleared) {
            gameObject.SetActive(false);
        }
        _textSet = false;
	_textCleared = false;
    }

    public void SetText(string text) {
        _textSet = true;
        gameObject.SetActive(true);
        textObject.text = text;
    }

    public void ClearText() {
        _textCleared = true;
    }
}
