using System;
using FullSerializer;

public enum AttackType {
    Melee,
    Ranged,
    Magic
}

/// <summary>
/// Maps each attack type to an int
/// </summary>
[fsObject(Converter = typeof(ArmorClassConverter))]
public class ArmorClass : Enumap<AttackType, int> { }
class ArmorClassConverter : DictConverter<ArmorClass> { }