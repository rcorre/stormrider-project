using System.Collections.Generic;
using UnityEngine;

public class UseFeat : State<Battle> {
    private Battler _battler;
    private Tile _target;
    private Feat _feat;

    public UseFeat(Battler battler, Tile target, Feat feat) {
        _battler = battler;
        _target = target;
        _feat = feat;
    }

    public override void Start(Battle battle) {
	// this is a transient state, pop it right off the stack
	battle.states.Pop();

	// compute effects
        var effects = BattleCalc.CalculateEffects(_feat, _battler, _target.battler);
	var targetBattler = _target.battler;
	if (targetBattler == null) {
	    Debug.LogError("cannot handle feat used on empty tile yet");
	}

	// push a state to apply each effect
	foreach(var effect in effects) {
	    battle.states.Push(new ApplyFeatEffect(targetBattler, effect));
	}
    }
}
