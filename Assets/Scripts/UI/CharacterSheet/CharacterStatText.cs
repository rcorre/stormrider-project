using UnityEngine;
using UnityEngine.UI;

public enum CharacterStat {
    BaseAttribute,
    EffectiveAttribute,
}

[RequireComponent(typeof(Text))]
public class CharacterStatText : MonoBehaviour {
    public CharacterStat stat;
    public CharacterAttribute attribute;
    private Character _character;
    private Text _text;

    // Use this for initialization
    void Start() {
        _character = FindObjectOfType<CharacterSheetUI>().character;
        _text = GetComponent<Text>();
        SetText();
    }

    public void SetText() {
        int val = _character.baseAttributes[attribute];
        _text.text = val.ToString();
    }
}
