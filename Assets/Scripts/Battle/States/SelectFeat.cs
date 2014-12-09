using System.Collections.Generic;
using UnityEngine;

public class SelectFeat : State<Battle> {
    private Battler _actor;
    private Tile _target;
    private FeatSelector _selector;

    public SelectFeat(Battler battler, Tile target) {
        _actor = battler;
        _target = target;
    }

    public override void Start(Battle battle) {
        base.Start(battle);
	// TODO: select based on enemy/ally/range
        var pos = battle.map.mesh.TileSurfaceCenter(_target);
        var featOptions = _actor.character.feats;

        _selector = GameObject.FindObjectOfType<FeatSelector>();
        _selector.Show(pos, featOptions, feat => Choose(battle, feat));
    }

    public override void End(Battle obj) {
        base.End(obj);
        _selector.Hide();
    }

    private void Choose(Battle battle, Feat feat) {
	battle.states.Replace(new UseFeat(_actor, _target, feat));
    }
}
