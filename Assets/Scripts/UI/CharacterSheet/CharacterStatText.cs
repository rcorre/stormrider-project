using UnityEngine;
using UnityEngine.UI;

public enum CharacterStat {
    BaseAttribute,
    EffectiveAttribute,
    ArmorClass
}

[RequireComponent(typeof(Text))]
public class CharacterStatText : MonoBehaviour {
    const string armorClassFormat = "{0} - {1}"; // min - max
    public CharacterStat stat;
    public CharacterAttribute attribute;
    public Element element;
    private Character _character;
    private Text _text;

    // Use this for initialization
    void Start() {
        _character = FindObjectOfType<CharacterSheetUI>().character;
        _text = GetComponent<Text>();
        SetText();
    }

    public void SetText() {
        string txt = "";
        switch (stat) {
            case CharacterStat.BaseAttribute:
                txt = _character.baseAttributes[attribute].ToString();
                break;
            case CharacterStat.EffectiveAttribute:
                txt = _character.effectiveAttributes[attribute].ToString();
                break;
            case CharacterStat.ArmorClass:
                txt = string.Format(armorClassFormat, 
                    _character.minArmorClass[element], _character.maxArmorClass[element]);
                break;
            default:
                break;
        }
        _text.text = txt;
    }
}
