using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleGUI : MonoBehaviour {
    public enum TextType {
	Damage,
	Healing
    }

    public TextPopup DamageText, HealingText;

    void Start() {
    }

    public void SpawnText(string text, TextType type, Vector3 worldPosition) {
        var position = Camera.main.WorldToViewportPoint(worldPosition);
        var textPrefab = DamageText;
        switch (type) {
            case TextType.Damage:
                textPrefab = DamageText;
                break;
            case TextType.Healing:
                textPrefab = HealingText;
                break;
            default:
                break;
        }
        var popup = (TextPopup)GameObject.Instantiate(textPrefab, position, Quaternion.identity);
        popup.guiText.text = text;
    }

    public void SpawnText(int value, TextType type, Vector3 worldPosition) {
        SpawnText(value.ToString(), type, worldPosition);
    }

    public void SpawnText(EffectResult result, Vector3 worldPosition) {
        TextType textType = TextType.Damage;
        switch (result.effect) {
            case TalentEffect.Damage:
            case TalentEffect.DamageStamina:
            case TalentEffect.ApplyCondition:
                textType = TextType.Damage;
                break;
            case TalentEffect.Heal:
            case TalentEffect.RestoreStamina:
                textType = TextType.Healing;
                break;
        }
        SpawnText(result.ToString(), textType, worldPosition);
    }
}