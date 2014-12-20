public enum WeaponStyle {
    OneHanded,
    Versatile,
    TwoHanded
}

public enum AmmoType {
    None,
    Arrow,
    Bolt,
    Stone
}

public abstract class ItemData {
    public readonly string key; /// <summary> unique id, unchanged even if name changes </summary>
    public readonly string name; /// <summary> name shown in ui </summary>

    public readonly int weight;
    public readonly int value;
}

public class EquipmentData : ItemData {
    public readonly ArmorClass armorClass;
    public readonly int resistance;
}

public class ArmorData : EquipmentData {
    public readonly string slot;
}

public class WeaponData : EquipmentData {
    public readonly WeaponStyle style;
    public readonly AttackType attackType;
    public readonly Element element;
    public readonly AmmoType ammoType;
    public readonly DiceRoll damage;
    public readonly int accuracy;
    public readonly int minRange;
    public readonly int maxRange;
}