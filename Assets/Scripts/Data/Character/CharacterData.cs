using System.Linq;
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
public class AttributeSetConverter : DictConverter<AttributeSet> { }

public class Character {
    #region const
    /// <summary>
    /// number of dex for +1 physical ac, amount of wis for +1 magical ac
    /// </summary>
    const int apPerAc         = 3;
    const int hpPerCon        = 1;
    const int staminaPerCon   = 1;
    const int conPerFortitude = 2;
    const int wisPerWillpower = 2;
    const int weightCapacityPerStr = 2;
    const int weightCapacityPerCon = 1;
    #endregion

    #region base values
    public string name { get; private set; }
    public Race race { get; private set; }
    public int level { get; private set; }
    public int xp { get; private set; }

    public AttributeSet baseAttributes { get; private set; }
    public Feat[] feats { get; private set; }
    public Archetype[] archetypes { get; private set; }

    public Weapon mainHand { get; private set; }
    public Weapon offHand { get; private set; }
    public Armor[] armor { get; private set; }
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
    #endregion

    #region dynamic values
    public int health { get; private set; }
    public int stamina { get; private set; }
    #endregion

    #region public methods
    #endregion

    #region private methods
    void RecalculateStats() {
	// attributes
        effectiveAttributes = baseAttributes;

	// weight
        weightCapacity = race.weightCapacity +
            effectiveAttributes[CharacterAttribute.Str] * weightCapacityPerStr +
            effectiveAttributes[CharacterAttribute.Con] * weightCapacityPerCon;

        weightCarried = armor.Sum(x => x.weight) + mainHand.weight + offHand.weight;

	// hp and stamina
        maxHealth  = race.health  + effectiveAttributes[CharacterAttribute.Con] * hpPerCon;
        maxStamina = race.stamina + effectiveAttributes[CharacterAttribute.Con] * staminaPerCon;
        health  = Mathf.Clamp(health,  0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

	// fortitude and willpower
        fortitude = race.fortitude + effectiveAttributes[CharacterAttribute.Con] / conPerFortitude;
        willpower = race.willpower + effectiveAttributes[CharacterAttribute.Wis] / wisPerWillpower;

	// min and max armor class
        foreach (var element in ElementSet.EnumKeys) {
            var attr = element.IsPhysical() ? CharacterAttribute.Dex : CharacterAttribute.Wis;
            minArmorClass[element] = effectiveAttributes[attr] / apPerAc + race.baseArmor[element];
            int maxAc = 0;
            foreach (var armorPiece in armor) {
                maxAc += armorPiece.armorClass[element];
            }
            maxArmorClass[element] = maxAc;
        }
    }
    #endregion
}
