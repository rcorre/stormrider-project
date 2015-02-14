public enum ConditionType {
    Bleed,
    Burn,
    Poison,
    Blind,
    Slow,
}

public class ConditionSet : Enumap<ConditionType, int> { }