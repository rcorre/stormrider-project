using UnityEngine;

public class UnitSelected : State<Battle> {
    private Battler _battler;
    private PathFinder _pathFinder;
    private TileOverlay _overlay;
    private Tile[] _moveRange;

    public UnitSelected(Battler battler) {
        _battler = battler;
    }

    public override void Start(Battle battle) {
        base.Start(battle);
        _pathFinder = new PathFinder(battle.map, _battler);
	_moveRange = _pathFinder.TilesInRange;
	_overlay = GameObject.FindObjectOfType<TileOverlay>();
	_overlay.HighlightTiles(_moveRange, TileOverlay.HighlightType.Move);
    }

    public override void Update(Battle battle) {
        base.Update(battle);
        var tile = battle.map.mouse.tileUnderMouse;
    }
}
