using System;
using System.Collections.Generic;
using FullSerializer;

[fsObject(Converter=typeof(WeaponConverter))]
public class Weapon : Equipment {
    public WeaponStyle style { get; private set; }
    public AttackType attackType { get; private set; }
    public Element element { get; private set; }
    public DiceRoll damage { get; private set; }
    public int accuracy { get; private set; }
    public AmmoType ammoType { get; private set; }

    public Weapon(WeaponData model, EquipmentMaterial material)
        : base(model, material) 
    {
        style      = model.style;
        attackType = model.attackType;
        element    = model.element;
        damage     = model.damage * material.damage[element];
        accuracy   = model.accuracy;
        ammoType   = model.ammoType;
    }
}

public class WeaponConverter : fsConverter {
    public override bool CanProcess(Type type) {
        return type == typeof(Weapon);
    }

    public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType) {
        var input = (Weapon)instance;

        var output = new Dictionary<string, fsData>() {
	    { "model", new fsData(input.model.key) },
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
        var model = DataManager.Fetch<WeaponData>(input["model"].AsString);
        var material = DataManager.Fetch<EquipmentMaterial>(input["material"].AsString);
        instance = new Weapon(model, material);
        return fsFailure.Success;
    }

    public override object CreateInstance(fsData data, Type storageType) {
        return null;
    }
}