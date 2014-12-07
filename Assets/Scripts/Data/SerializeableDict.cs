using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

/// <summary>
/// thouroughly untested. expect nothing
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
public abstract class SerializeableDict<K,V> : Dictionary<K,V>, IJsonDict {
    public void SetValue(string key, fsData val) {
        object result = null;
        switch (val.Type) {
            case fsDataType.Array:
                result = val.AsList;
                break;
            case fsDataType.Object:
                result = val.AsDictionary;
                break;
            case fsDataType.Double:
                result = val.AsDouble;
                break;
            case fsDataType.Int64:
                result = val.AsInt64;
                break;
            case fsDataType.Boolean:
		result = val.AsBool;
                break;
            case fsDataType.String:
                result = val.AsString;
                break;
            case fsDataType.Null:
                result = null;
                break;
            default:
                break;
        }
        var k = (K)Convert.ChangeType(key, typeof(K));
        var v = (V)Convert.ChangeType(result, typeof(V));
        this[k] = v;
    }

    public fsData GetValue(string key) {
        var k = (K)Convert.ChangeType(key, typeof(K));
        var v = this[k];
        Type valType = typeof(V);

        if (typeof(double).IsAssignableFrom(valType)) {
            return new fsData((double)Convert.ChangeType(v, typeof(double)));
        }
        else if (typeof(int).IsAssignableFrom(valType)) {
            return new fsData((int)Convert.ChangeType(v, typeof(int)));
        }
        else if (typeof(string).IsAssignableFrom(valType)) {
            return new fsData((string)Convert.ChangeType(v, typeof(string)));
        }
        else if (typeof(bool).IsAssignableFrom(valType)) {
            return new fsData((bool)Convert.ChangeType(v, typeof(bool)));
        }
        else {
            Util.Assert(false, "SerializeableDict can't handle nested Lists or Dicts yet");
            return null;
        }
    }

    public new string[] Keys { get { return Enum.GetNames(typeof(K)); } }
}
