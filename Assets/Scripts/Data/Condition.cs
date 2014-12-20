public abstract class Condition {
    public int amount;
}

/// <summary>
/// conditions that have 'special handling'
/// </summary>
public class SpecialCondition : Condition {
    public enum Type {
        Burn,
        Toxic,
        Bleed,
        Break,
    }

    public Type type;
}

/// <summary>
/// adjust a character attribute by a constant amount
/// </summary>
public class AttributeCondition : Condition {
    public CharacterAttribute attribute;
}
