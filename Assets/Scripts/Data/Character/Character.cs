using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public enum CharacterAttribute {
    Str,
    Dex,
    Con,
    Int,
    Wis,
    Cha
}

[fsObject(Converter = typeof(AttributeSetConverter))]
public class AttributeSet : Enumap<CharacterAttribute, int> { }
class AttributeSetConverter : DictConverter<AttributeSet> { }

public class Character {
    #region const
    /// <summary>
    /// number of dex for +1 physical ac, amount of wis for +1 magical ac
    /// </summary>
    const int apPerAc              = 3;
    const int hpPerCon             = 1;
    const int staminaPerCon        = 1;
    const int conPerFortitude      = 2;
    const int wisPerWillpower      = 2;
    const int dexPerInitiative	   = 4;
    const int weightCapacityPerStr = 2;
    const int weightCapacityPerCon = 1;
    #endregion

    #region base values
    [fsProperty(Name="race")]
    private string raceKey {
        get { return race.key; }
        set { race = DataManager.Fetch<Race>(value); }
    }
    [fsIgnore]
    public Race race { get; private set; }

    [fsProperty]
    public string name { get; private set; }
    [fsProperty]
    public int level { get; private set; }
    [fsProperty]
    public int xp { get; private set; }

    [fsProperty]
    public AttributeSet baseAttributes { get; private set; }
    [fsProperty]
    public Feat[] feats { get; private set; }
    [fsProperty]
    public Archetype[] archetypes { get; private set; }

    [fsProperty]
    public Weapon mainHand { get; private set; }
    [fsProperty]
    public Weapon offHand { get; private set; }
    [fsProperty]
    public Armor[] armor { get; private set; }
    #endregion

    #region dynamic values
    public int health { get; private set; }
    public int stamina { get; private set; }

    public List<Condition> conditions { get; private set; }
    #endregion

    #region calculated stats
    public AttributeSet effectiveAttributes { get; private set; }
    public ElementSet minArmorClass { get; private set; }
    public ElementSet maxArmorClass { get; private set; }
    public int maxHealth { get; private set; }
    public int maxStamina { get; private set; }
    public int fortitude { get; private set; }
    public int willpower { get; private set; }
    public int weightCapacity { get; private set; }
    public int weightCarried { get; private set; }
    public float attunement { get; private set; }
    public int initiative { get; private set; }

    public int speed { get { return race.speed; } }
    #endregion

    #region public methods
    public void FullRestore() {
        health = maxHealth;
        stamina = maxStamina;
    }

    public void RecalculateStats() {
	// attributes
        effectiveAttributes = baseAttributes;

	// weight and attunement
        weightCapacity = race.weightCapacity +
            effectiveAttributes[CharacterAttribute.Str] * weightCapacityPerStr +
            effectiveAttributes[CharacterAttribute.Con] * weightCapacityPerCon;

        weightCarried = armor.Sum(x => x.weight) + mainHand.weight + offHand.weight;
        attunement    = armor.Aggregate(1f, (total, piece) => total * piece.attunement);

	// hp and stamina
        maxHealth  = race.health  + effectiveAttributes[CharacterAttribute.Con] * hpPerCon;
        maxStamina = race.stamina + effectiveAttributes[CharacterAttribute.Con] * staminaPerCon;
        health  = Mathf.Clamp(health,  0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

	// fortitude and willpower
        fortitude = race.fortitude + effectiveAttributes[CharacterAttribute.Con] / conPerFortitude;
        willpower = race.willpower + effectiveAttributes[CharacterAttribute.Wis] / wisPerWillpower;

	// initiative
        initiative = race.initiative + effectiveAttributes[CharacterAttribute.Dex] / dexPerInitiative;

	// min and max armor class
        minArmorClass = new ElementSet();
        maxArmorClass = new ElementSet();
        foreach (var element in ElementSet.EnumKeys) {
            var attr = element.IsPhysical() ? CharacterAttribute.Dex : CharacterAttribute.Wis;
            minArmorClass[element] = effectiveAttributes[attr] / apPerAc + race.baseArmor[element];
            int maxAc = 0;
            foreach (var armorPiece in armor) {
                maxAc += armorPiece.armorClass[element];
            }
            maxArmorClass[element] = Mathf.Max(maxAc, minArmorClass[element]);
        }
    }
    #endregion

    #region private methods
    #endregion
}
