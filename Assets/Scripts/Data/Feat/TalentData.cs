using FullSerializer;
using System.Collections.Generic;

public enum ActionType {
    Major,
    Minor,
    Move
}

public enum TalentType {
    Melee,
    Ranged,
    Magic
}

public abstract class TalentData {
    /// <summary> unique id, unchanged even if name changes </summary>
    public readonly string key;
    /// <summary> name shown in ui </summary>
    public readonly string name;
    /// <summary> how talent may be used in combat </summary>
    public readonly TalentType type; 
    /// <summary> type of action consumed </summary>
    public readonly ActionType action; 
    /// <summary> attribute required to equip </summary>
    public readonly CharacterAttribute attribute;
    /// <summary> number of attribute points required to equip </summary>
    public readonly int apCost;
    /// <summary> number of turns before talent can be used again after use </summary>
    public readonly int cooldown;
    /// <summary> stamina consumed on use </summary>
    public readonly int stamina;
}