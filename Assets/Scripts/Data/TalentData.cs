using FullSerializer;
using System.Collections.Generic;

public enum TalentType {
    Major,
    Minor,
    Technique,
    Effect,
    Trigger
}

public enum TalentRequirement {
    None,
    Melee, // any melee weapon
    Unarmed,
    Blade,
    Blunt,
    Piercing,
    Shield,
    Ranged, // any ranged weapon
    Bow,
    Gun,
    Thrown,
    Magic, // any magic weapon
    Force,
    Mind,
    Shout
}

public enum StatusEffect {
    Bleeding,
    Burning,
    Poison,
    Cripple,
    Blind,
    Slow,
    Fatigue
}

public enum AOEType {
    Single,
    Burst,
    Cone,
    Line,
    Blast
}

public class TalentData {
    public readonly string key; /// <summary> unique id, unchanged even if name changes </summary>
    public readonly string name; /// <summary> name shown in ui </summary>
    public readonly TalentType type;
    public readonly TalentRequirement requirement;
    //costs
    public readonly CharacterAttribute attribute;
    public readonly int apCost;
    public readonly int cooldown;
    public readonly int stamina;
    // trajectory
    public readonly AOEType aoe;
    public readonly int range;
    // modifiers
    public readonly int toHitMod;
    // effects
    public readonly int damage;
    public readonly int healing;
    public readonly StatusEffect statusEffect;
}