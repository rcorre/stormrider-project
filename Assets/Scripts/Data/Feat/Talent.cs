using UnityEngine;
using FullSerializer;

public struct Talent {
    public readonly int rank;
    // when the key is set, use it to load a reference to the talent data
    [SerializeField]
    public string key {
        get { return  data.key; }
        private set { data = DataManager.Fetch<TalentData>(value); }
    }
    [fsIgnore]
    public TalentData data { get; private set; }
}