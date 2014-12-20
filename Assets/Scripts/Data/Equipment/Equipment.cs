using UnityEngine;
using System;
using System.Linq;

public abstract class Equipment {
    const string toStringFmt = "{0} {1}"; // {model} {material}

    public int weight { get; private set; }
    public int value { get; private set; }
    public ArmorClass armorClass { get; private set; }
    public ElementSet resistance { get; private set; }

    public EquipmentData model { get; private set; }
    public EquipmentMaterial material { get; private set; }

    public Equipment(EquipmentData model, EquipmentMaterial material) {
        this.model = model;
        this.material = material;

        weight = (int)(model.weight * material.weight);
        value = (int)(model.value * material.value);
        armorClass = model.armorClass;

        resistance = new ElementSet();
	foreach(var el in ElementSet.EnumKeys) {
	    resistance[el] = (int)(model.resistance * material.resist[el]);
	}
    }

    public override string ToString() {
        return string.Format(toStringFmt, material.name, model.name);
    }
}