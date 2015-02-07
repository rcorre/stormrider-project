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
        var gui = GameObject.FindObjectOfType<BattleGUI>();
        gui.SpawnText("Testing", BattleGUI.TextType.Damage, _target.transform.position);
    }

    public override void Update(Battle battle) {
    }

    public override void Exit(Battle obj) {
    }
}
