using UnityEngine;
using System.Collections;

public class CharacterSheetUI : MonoBehaviour {
    public string characterName;

    private Character _character;
    public Character character {
        get {
            if (_character == null && characterName != null) {
                _character = DataManager.Fetch<Character>(characterName);
            }
            return _character;
        }
    }
}