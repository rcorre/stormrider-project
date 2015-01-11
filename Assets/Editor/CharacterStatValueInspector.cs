using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
[CustomEditor(typeof(CharacterStatText))]
public class CharacterStatTextEditor : Editor {
    public override void OnInspectorGUI() {
        var statText = (CharacterStatText)target;

        EditorGUILayout.BeginVertical();

        statText.stat = selectEnum<CharacterStat>(statText.stat);
        switch (statText.stat) {
            case CharacterStat.BaseAttribute:
            case CharacterStat.EffectiveAttribute:
                statText.attribute = selectEnum<CharacterAttribute>(statText.attribute);
                break;
            case CharacterStat.ArmorClass:
                statText.element = selectEnum<Element>(statText.element);
                break;
        }

        EditorGUILayout.EndVertical();
    }

    private T selectEnum<T>(Enum current) {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel(typeof(T).ToString());
	var selection = EditorGUILayout.EnumPopup(current);

        EditorGUILayout.EndHorizontal();

        return (T)Convert.ChangeType(selection, typeof(T));
    }
}
