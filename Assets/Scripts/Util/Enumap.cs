using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

public abstract class Enumap<K,V> : IJsonDict {
    public readonly int size;
    private V[] _store;

    public Enumap() {
        Util.Assert(typeof(K).IsEnum, "Enumap expects T : enum, got " + typeof(K));
	size = Enum.GetValues(typeof(K)).Length;
	_store = new V[size];
    }

    public V this[K key] {
        get { return _store[Convert.ToInt32(key)]; }
        set { _store[Convert.ToInt32(key)] = value; }
    }

    public void SetValue(string key, fsData val) {
        int idx = (int)Enum.Parse(typeof(K), key, true);
        V entry = default(V);
        if (val.Type == fsDataType.Int64) {
            entry = (V)Convert.ChangeType(val.AsInt64, typeof(V));
        }
        else if (val.Type == fsDataType.Double) {
            entry = (V)Convert.ChangeType(val.AsDouble, typeof(V));
        }
        else {
            Util.Assert(false, "Enumap value can only be assigned from int64 or double");
        }
        _store[(int)idx] = entry;
    }

    public fsData GetValue(string key) {
        return new fsData();
    }

    public string[] Keys { get { return Enum.GetNames(typeof(K)); } }

    public static IEnumerable<K> EnumKeys {
        get { return Enum.GetValues(typeof(K)).Cast<K>(); }
    }
}
