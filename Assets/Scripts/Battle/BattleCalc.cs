using System;
using System.Linq;

public static class BattleCalc {

    #region Calculations

    public static int FeatDamage(Feat feat, Battler user, Battler target) {
        switch (feat.technique.data.type) {
            case TechniqueType.Melee:
                return MeleeDamage(feat, user, target);
            case TechniqueType.Ranged:
                return RangedDamage(feat, user, target);
            case TechniqueType.Magic:
                return MagicDamage(feat, user, target);
            default:
                throw new NotImplementedException();
        }
    }

    #endregion

    #region Helpers

    private static int MeleeDamage(Feat feat, Battler user, Battler target) {
        var power = user.character.mainHand.damage.Roll();
        return power;
    }

    private static int RangedDamage(Feat feat, Battler user, Battler target) {
        throw new NotImplementedException();
    }

    private static int MagicDamage(Feat feat, Battler user, Battler target) {
        throw new NotImplementedException();
    }

    #endregion
}
