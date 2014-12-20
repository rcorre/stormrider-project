public enum EffectType {
    Health,
    Stamina,
    Bleed,
    Burn,
    Poison,
    Blind,
    Slow,
}

public abstract class Condition {
    public int potency;
}