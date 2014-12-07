using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using FullSerializer;

[fsObject(Converter=typeof(DiceRollConverter))]
public struct DiceRoll {
    const string StringFormat = "{0}d{1} + {2}";
    static readonly Regex RollRegex;
    private int _numDice; 
    private int _numSides; 
    private int _bonus;

    static DiceRoll() {
	RollRegex = new Regex(@"^(?<dice>\d+)\s*d\s*(?<sides>\d+)\s*[+]\s*(?<bonus>\d+)");
    }

    /// <summary>
    /// construct a dice roll from a string of format nds + b
    /// </summary>
    public DiceRoll(string spec) {
        Match match = RollRegex.Match(spec);
        if (match == Match.Empty) { 
            Debug.LogError("could not parse dice roll from " + spec); 
        }

        _numDice  = int.Parse(match.Groups["dice"].Value);
        _numSides = int.Parse(match.Groups["sides"].Value);
        _bonus    = int.Parse(match.Groups["bonus"].Value);
    }

    public DiceRoll(int numDice, int numSides, int bonus) {
        _numDice = numDice;
        _numSides = numSides;
        _bonus = bonus;
    }

    /// <summary>
    /// convert a dice roll to a string of format nds + b
    /// </summary>
    public override string ToString() {
        return String.Format(StringFormat, _numDice, _numSides, _bonus);
    }

    /// <summary>
    /// generate a result from this dice roll
    /// </summary>
    public int Roll() {
        int sum = _bonus;
        for (int i = 0; i < _numDice; i++) {
	    // Random.Range for ints is [min, max), hence the +1
            sum += UnityEngine.Random.Range(1, _numSides + 1);
        }
        return sum;
    }

    public static DiceRoll operator *(DiceRoll roll, float factor) {
        int numSides = Mathf.FloorToInt(roll._numSides * factor);
        int bonus = Mathf.FloorToInt(roll._bonus * factor);
        return new DiceRoll(roll._numDice, numSides, bonus);
    }
}

/// <summary>
/// parse a dice roll from a string of form "nds + b"
/// </summary>
public class DiceRollConverter : fsConverter {
    public override bool CanProcess(Type type) {
        return type == typeof(DiceRoll);
    }

    public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType) {
        serialized = new fsData(instance.ToString());
        return fsFailure.Success;
    }

    public override fsFailure TryDeserialize(fsData storage, ref object instance, Type storageType) {
        // verify json contains expected type
        if (storage.Type != fsDataType.String) {
            return fsFailure.Fail("Expected string fsData type but got " + storage.Type);
        }

        instance = new DiceRoll(storage.AsString);
        return fsFailure.Success;
    }
}
