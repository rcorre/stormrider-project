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
    /// <summary> unique id, unchanged even if name changes </summary>
    public readonly string key;
    /// <summary> name shown in ui </summary>
    public readonly string name;

    public readonly int weight;
    public readonly int value;
}

public class EquipmentDesign : ItemData {
    public readonly string slot;
    public readonly float attunement;
    public readonly int armorClass;
}

public class ArmorDesign : EquipmentDesign {
}

public class WeaponDesign : EquipmentDesign {
    public readonly CharacterAttribute attribute;
    public readonly WeaponStyle style;
    public readonly Element element;
    public readonly AmmoType ammoType;
    public readonly DiceRoll damage;
    public readonly int minRange;
    public readonly int maxRange;
}