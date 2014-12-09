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
        base.Start(battle);
    }

    public override void Update(Battle battle) {
        base.Update(battle);
    }

    public override void Exit(Battle obj) {
        base.Exit(obj);
    }
}
