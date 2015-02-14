using System.Collections.Generic;
using UnityEngine;

public class ApplyFeatEffect : State<Battle> {
    private Battler _battler;
    private EffectResult _result;

    public ApplyFeatEffect(Battler battler, EffectResult result) {
        _battler = battler;
	_result = result;
    }

    public override void Start(Battle battle) {
        var gui = GameObject.FindObjectOfType<BattleGUI>();
        var map = GameObject.FindObjectOfType<TileMap>();
        var pos = map.mesh.TileSurfaceCenter(_battler.tile);

        gui.SpawnText(_result, pos);
    }

    public override void Update(Battle battle) {
    }

    public override void Exit(Battle obj) {
    }
}
