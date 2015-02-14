using System;
using System.Collections.Generic;
using FullSerializer;

[fsObject(Converter = typeof(TalentConverter))]
public struct Talent {
    public Talent(int rank, TalentData data) {
        this.rank = rank;
        this.data = data;
    }
    public readonly int rank;
    public readonly TalentData data;
}

public class TalentConverter : fsConverter {
    public override bool CanProcess(Type type) {
        return type == typeof(Talent);
    }

    public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType) {
        var input = (Talent)instance;
        var output = new Dictionary<string, fsData>() {
	    { "rank", new fsData(input.rank) },
	    { "key", new fsData(input.data.key) }
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

        int rank   = (int) input["rank"].AsInt64;
        string key = input["key"].AsString;

        var data = DataManager.Fetch<TalentData>(key);

        instance = new Talent(rank, data);
        return fsFailure.Success;
    }

    public override object CreateInstance(fsData data, Type storageType) {
        return new Talent();
    }
}