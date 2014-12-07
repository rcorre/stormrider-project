using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

using TalentStore = System.Collections.Generic.Dictionary<string,TalentData>;
using EquipmentStore = System.Collections.Generic.Dictionary<string,EquipmentData>;
using MaterialStore = System.Collections.Generic.Dictionary<string,EquipmentMaterial>;

public class DataManager : MonoBehaviour {
    /// <summary>
    /// objects of type T are accessed as _store[T][key]
    /// </summary>
    private static Dictionary<Type, Dictionary<string, object>> _store;

    void Awake() {
        _store = new Dictionary<Type, Dictionary<string, object>>();
	// order matters, cannot load characters before talents or equipment
        Load<TalentData>("talents", x => x.key);
        Load<EquipmentMaterial>("materials", x => x.key);
        Load<WeaponData>("weapons", x => x.key);
        Load<ArmorData>("armor", x => x.key);
        Load<CharacterData>("characters", x => x.name);
    }

    /// <summary>
    /// load and cache data of type T
    /// </summary>
    /// <typeparam name="T">Type to deserialize into</typeparam>
    /// <param name="fileName">name of file under Resources, without extension</param>
    /// <param name="getKey">key on which to index store</param>
    public static void Load<T>(string fileName, Func<T, string> getKey) {
	var asset = Resources.Load<TextAsset>(fileName);
	var data = JsonApi.Deserialize<T[]>(asset.text);
	_store[typeof(T)] = data.ToDictionary(x => getKey(x), x => (object)x);
    }

    public static T Fetch<T>(string key) {
	Type type = typeof(T);
	Util.Assert(_store.ContainsKey(type), "no data of type " + type + "has been loaded");
	Util.Assert(_store[type].ContainsKey(key), "key " + key + " not found in store of " + type);
        return (T)_store[typeof(T)][key];
    }

    public static IEnumerable<T> FetchAll<T>() {
	Type type = typeof(T);
	Util.Assert(_store.ContainsKey(type), "no data of type " + type + "has been loaded");
        return _store[typeof(T)].Values.Cast<T>();
    }
}