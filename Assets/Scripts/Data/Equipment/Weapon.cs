using System;
using System.Collections.Generic;
using FullSerializer;

[fsObject(Converter=typeof(WeaponConverter))]
public class Weapon : Equipment {
    public readonly Element element;
    public readonly AmmoType ammoType;
    public readonly WeaponStyle style;
    public readonly int minRange, maxRange;
    public readonly DiceRoll damage;
    public readonly CharacterAttribute attribute;

    public Weapon(WeaponDesign design, EquipmentMaterial material)
        : base(design, material) 
    {
        element   = design.element;
        ammoType  = design.ammoType;
        style     = design.style;
        minRange  = design.minRange;
        maxRange  = design.maxRange;
        damage    = design.damage * material.attack[element];
        attribute = design.attribute;
    }
}

public class WeaponConverter : fsConverter {
    public override bool CanProcess(Type type) {
        return type == typeof(Weapon);
    }

    public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType) {
        var input = (Weapon)instance;

        var output = new Dictionary<string, fsData>() {
	    { "model", new fsData(input.design.key) },
	    { "material", new fsData(input.material.key) }
        };

        serialized = new fsData(output);
        return fsFailure.Success;
    }

    public override fsFailure TryDeserialize(fsData storage, ref object instance, Type storageType) {
        // verify json contains expected type
        if (storage.Type != fsDataType.Object) {
            return fsFailure.Fail("Expected object fsData type but got " + storage.Type);
        }

        var input = storage.AsDictionary;
        if (!input.ContainsKey("design")) { UnityEngine.Debug.Log(storage.AsString); }
        var design = DataManager.Fetch<WeaponDesign>(input["design"].AsString);
        var material = DataManager.Fetch<EquipmentMaterial>(input["material"].AsString);
        instance = new Weapon(design, material);
        return fsFailure.Success;
    }

    public override object CreateInstance(fsData data, Type storageType) {
        return null;
    }
}