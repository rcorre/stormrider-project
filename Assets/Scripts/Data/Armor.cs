using System;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;

[fsObject(Converter=typeof(ArmorConverter))]
public class Armor : Equipment {
    public string slot { get; private set; }

    public Armor(ArmorData model, EquipmentMaterial material)
        : base(model, material) 
    {
        slot = model.slot;
    }
}

public class ArmorConverter : fsConverter {
    public override bool CanProcess(Type type) {
        return type == typeof(Weapon);
    }

    public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType) {
        var input = (Armor)instance;

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
        var model = DataManager.Fetch<ArmorData>(input["model"].AsString);
        var material = DataManager.Fetch<EquipmentMaterial>(input["material"].AsString);
        instance = new Armor(model, material);
        return fsFailure.Success;
    }

    public override object CreateInstance(fsData data, Type storageType) {
        return null;
    }
}