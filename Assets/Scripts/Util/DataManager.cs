using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

public static class DataManager {
    /// <summary>
    /// objects of type T are accessed as _store[T][key]
    /// </summary>
    private static Dictionary<Type, Dictionary<string, object>> _store;

    static DataManager() {
        _store = new Dictionary<Type, Dictionary<string, object>>();
	// order matters, cannot load characters before talents or equipment
        Load<Race>("race", x => x.key);
        Load<TalentData>("talents/technique", x => x.key);
        Load<TalentData>("talents/effect", x => x.key);
        Load<EquipmentMaterial>("materials", x => x.key);
        Load<WeaponDesign>("weapons", x => x.key);
        Load<ArmorDesign>("armor", x => x.key);
        Load<Character>("characters", x => x.name);
    }

    /// <summary>
    /// load and cache data of type T.
    /// if some data of type T has already been loaded, merges with existing store.
    /// duplicate keys for a given type are not allowed.
    /// </summary>
    /// <typeparam name="T">Type to deserialize into</typeparam>
    /// <param name="fileName">name of file under Resources, without extension</param>
    /// <param name="getKey">key on which to index store</param>
    public static void Load<T>(string fileName, Func<T, string> getKey) {
	var asset = Resources.Load<TextAsset>(fileName);
	var data = JsonApi.Deserialize<T[]>(asset.text);
	var typeKey = typeof(T);
    	if (_store.ContainsKey(typeKey)) { // key already exists, add pairs into existing dict
            data.ToList().ForEach(x => _store[typeKey].Add(getKey(x), (object)x));
    	}
    	else { // type not loaded yet. create new dictionary
            _store[typeKey] = data.ToDictionary(x => getKey(x), x => (object)x);
    	}
    }

    /// <summary>
    /// load data of type T, but do not cache it
    /// </summary>
    /// <typeparam name="T">Type to deserialize in to</typeparam>
    /// <param name="fileName">name of file under resources, without extension</param>
    /// <returns></returns>
    public static T LoadOnce<T>(string fileName) {
        var asset = Resources.Load<TextAsset>(fileName);
        return JsonApi.Deserialize<T>(asset.text);
    }

    /// <summary>
    /// retrieve data that has already been Loaded (and cached)
    /// </summary>
    /// <typeparam name="T">type data was deserialized into when loaded</typeparam>
    /// <param name="key">key of data within store</param>
    /// <returns>cached data matching key and type</returns>
    public static T Fetch<T>(string key) {
        return (T)Fetch(typeof(T), key);
    }

    /// <summary>
    /// retrieve data that has already been Loaded (and cached)
    /// </summary>
    /// <typeparam name="T">type data was deserialized into when loaded</typeparam>
    /// <param name="type">type of data class</param>
    /// <param name="key">key of data within store</param>
    /// <returns>cached data matching key and type</returns>
    public static object Fetch(Type type, string key) {
        Util.Assert(_store.ContainsKey(type), "no data of type " + type + " has been loaded");
        Util.Assert(_store[type].ContainsKey(key), "key " + key + " not found in store of " + type);
        return _store[type][key];
    }

    public static IEnumerable<T> FetchAll<T>() {
        Type type = typeof(T);
        Util.Assert(_store.ContainsKey(type), "no data of type " + type + "has been loaded");
        return _store[typeof(T)].Values.Cast<T>();
    }
}