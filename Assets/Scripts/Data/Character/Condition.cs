public enum ConditionType {
    AdjustAttribute,
    AdjustArmor,
    // TODO: more here
}

public struct Condition {
    ConditionType type;
    Element element;
    CharacterAttribute attribute;
    int amount;
}