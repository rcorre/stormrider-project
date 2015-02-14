using FullSerializer;
using System.Collections.Generic;

public enum TalentType {
    /// <summary> an action like strike, kick, shoot, cast </summary>
    Technique,
    /// <summary> determines element -- for magic feats only </summary>
    Affinity,
    /// <summary> damage, status effect, healing, buff, ect. </summary>
    Effect,
    /// <summary> a condition that will cause this feat to trigger automatically </summary>
    Reaction
}

public enum TalentEffect {
    Damage,
    Heal,
    DamageStamina,
    RestoreStamina,
    ApplyCondition,
}

public enum ActionType {
    Major,
    Minor,
    Move
}

public enum TechniqueType {
    Melee,
    Ranged,
    Magic
}

public class TalentData {
    #region General
    /// <summary> unique id, unchanged even if name changes </summary>
    public readonly string key;
    /// <summary> name shown in ui </summary>
    public readonly string name;
    /// <summary> the category of talent -- determines what slot it can be placed in </summary>
    public readonly TalentType talentType; 
    /// <summary> how talent may be used in combat </summary>
    public readonly TechniqueType techniqueType; 
    /// <summary> type of action consumed </summary>
    public readonly ActionType actionType; 
    /// <summary> attribute required to equip </summary>
    public readonly CharacterAttribute attribute;
    /// <summary> status condition afflicted </summary>
    public readonly ConditionType condition;
    /// <summary> number of attribute points required to equip </summary>
    public readonly int apCost;
    /// <summary> number of turns before talent can be used again after use </summary>
    public readonly int cooldown;
    /// <summary> stamina consumed on use </summary>
    public readonly int stamina;
    #endregion

    #region Effect Talents
    /// <summary> multiplied by the effective power to determine effect potency </summary>
    public readonly float power;
    /// <summary> multiplied by the effective power to determine effect potency </summary>
    public readonly TalentEffect effect;
    #endregion
}