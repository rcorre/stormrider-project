using UnityEngine;
using FullSerializer;

public enum CharacterRace {
    Human,
    Orc,
    Elf,
    Dwarf
}

public enum CharacterAttribute {
    Str,
    Dex,
    Con,
    Int,
    Wis,
    Cha
}

[fsObject(Converter = typeof(AttributeSetConverter))]
public class AttributeSet : Enumap<CharacterAttribute, int> { }
public class AttributeSetConverter : DictConverter<AttributeSet> { }

public struct CharacterData {
    public readonly string name;
    public readonly CharacterRace race;
    public readonly int level;
    public readonly int xp;
    public readonly AttributeSet attributes;
    public Feat[] feats;
    public Archetype[] archetypes;
    public Weapon mainHand;
    public Weapon offHand;
    public Armor[] armor;
}
