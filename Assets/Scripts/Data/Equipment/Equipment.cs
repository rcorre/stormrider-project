using UnityEngine;
using System;
using System.Linq;

public abstract class Equipment {
    const string toStringFmt = "{0} {1}"; // {model} {material}

    public readonly string slot;
    public readonly int weight;
    public readonly int value;
    public readonly float attunement;
    public readonly ElementSet armorClass;

    public readonly EquipmentDesign design;
    public readonly EquipmentMaterial material;

    public Equipment(EquipmentDesign design, EquipmentMaterial material) {
        this.design = design;
        this.material = material;

        slot       = design.slot;
        weight     = (int)(design.weight * material.weight);
        value      = (int)(design.value * material.value);
        attunement = design.attunement * material.attunement;

        armorClass = new ElementSet();
	foreach(var el in ElementSet.EnumKeys) {
	    armorClass[el] = (int)(design.armorClass * material.defense[el]);
	}
    }

    public override string ToString() {
        return string.Format(toStringFmt, material.name, design.name);
    }
}