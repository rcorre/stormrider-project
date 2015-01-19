using UnityEngine;
using UnityEngine.UI;

public enum CharacterStat {
    Name,
    Race,
    Level,
    Experience,
    Health,
    Stamina,
    Weight,
    Fortitude,
    Willpower,
    Initiative,
    Attunement,
    Speed,
    BaseAttribute,
    EffectiveAttribute,
    ArmorClass
}

[RequireComponent(typeof(Text))]
public class CharacterStatText : MonoBehaviour {
    const string armorClassFormat   = "{0} - {1}"; // min - max
    const string ratioFormat       = "{0}/{1}"; // min/max
    public CharacterStat stat;
    public CharacterAttribute attribute;
    public Element element;
    public bool showCurrent;
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
            case CharacterStat.Name:
                txt = _character.name;
                break;
            case CharacterStat.Race:
                txt = _character.race.name;
                break;
            case CharacterStat.Level:
                txt = _character.level.ToString();
                break;
            case CharacterStat.Attunement:
                txt = _character.attunement.ToString("0.0");
                break;
            case CharacterStat.Experience:
                txt = ratioText(_character.health, _character.maxHealth);
                break;
            case CharacterStat.Health:
                txt = ratioText(_character.health, _character.maxHealth);
                break;
            case CharacterStat.Stamina:
                txt = ratioText(_character.stamina, _character.maxStamina);
                break;
            case CharacterStat.Weight:
                txt = ratioText(_character.weightCarried, _character.weightCapacity);
                break;
            case CharacterStat.Fortitude:
                txt = _character.fortitude.ToString();
                break;
            case CharacterStat.Willpower:
                txt = _character.willpower.ToString();
                break;
            case CharacterStat.Initiative:
                txt = _character.initiative.ToString();
                break;
            case CharacterStat.Speed:
                txt = _character.speed.ToString();
                break;
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

    string ratioText(int currentVal, int maxVal) {
        return showCurrent ?
            string.Format(ratioFormat, currentVal, maxVal) :
            maxVal.ToString();
    }
}
