using System;
using System.Linq;
using System.Diagnostics;

public struct EffectResult {
    public TalentEffect effect;
    public ConditionType condition;
    public int amount;

    public override string ToString() {
        switch (effect) {
            case TalentEffect.Damage:
                return string.Format("HP - {0}", amount);
            case TalentEffect.Heal:
                return string.Format("HP + {0}", amount);
            case TalentEffect.DamageStamina:
                return string.Format("STA - {0}", amount);
            case TalentEffect.RestoreStamina:
                return string.Format("STA + {0}", amount);
            case TalentEffect.ApplyCondition:
                return string.Format("{0} + {1}", condition, amount);
        }

        throw new Exception(string.Format("Condition {0} not handled", condition));
    }
}

public static class BattleCalc {

    #region Calculations

    public static EffectResult[] CalculateEffects(Feat feat, Battler user, Battler target) {
        var power = EffectivePower(feat, user, target);
        return feat.effects.Select(x => CalculateEffect(x, power)).ToArray();
    }

    #endregion

    #region Helpers

    private static EffectResult CalculateEffect(Talent talent, int power) {
        Debug.Assert(talent.data.talentType == TalentType.Effect);
	EffectResult result;
	result.amount = power;
	result.effect = talent.data.effect;
	result.condition = talent.data.condition;
	return result;
    }

    private static int EffectivePower(Feat feat, Battler user, Battler target) {
        switch (feat.technique.data.techniqueType) {
            case TechniqueType.Melee:
                return user.character.mainHand.damage.Roll();
            case TechniqueType.Ranged:
                throw new NotImplementedException();
            case TechniqueType.Magic:
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }
    }

    #endregion
}
