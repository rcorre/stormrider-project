using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

/// <summary>
/// a dictionary-like type that can be serialized to and from a json object with Unity FullSerializer
/// </summary>
public interface IJsonDict {
    void SetValue(string key, fsData val);
    fsData GetValue(string key);
    string[] Keys { get; }
}

public abstract class DictConverter<T> : fsConverter where T : new() {
    public override bool CanProcess(Type type) {
        return type.GetInterfaces().Contains(typeof(IJsonDict));
    }

    public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType) {
        var input = (IJsonDict)instance;
        var output = new Dictionary<string, fsData>();

        foreach (var key in input.Keys) {
            output[key] = input.GetValue(key);
        }

        serialized = new fsData(output);
        return fsFailure.Success;
    }

    public override fsFailure TryDeserialize(fsData storage, ref object instance, Type storageType) {
        // verify json contains expected type
        if (storage.Type != fsDataType.Object) {
            return fsFailure.Fail("Expected object fsData type but got " + storage.Type);
        }

        var output = (IJsonDict)instance;
        var input = storage.AsDictionary;
        foreach (var kvp in input) {
            output.SetValue(kvp.Key, kvp.Value);
        }
        return fsFailure.Success;
    }

    public override object CreateInstance(fsData data, Type storageType) {
        return new T();
    }
}