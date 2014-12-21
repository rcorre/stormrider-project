using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

public class EquipmentMaterial {
    public readonly string key;
    public readonly string name;
    public readonly float weight;
    public readonly float value;
    /// <summary>
    /// multiplies damage for the corresponding element
    /// </summary>
    public readonly ElementMultiplier attack;
    /// <summary>
    /// multiplies resistance for the corresponding element
    /// </summary>
    public readonly ElementMultiplier defense;
    /// <summary>
    /// effectiveness as a spellcasting medium
    /// </summary>
    public readonly float attunement;
}
