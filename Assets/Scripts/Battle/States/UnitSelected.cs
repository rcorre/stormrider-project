using System.Collections.Generic;
using UnityEngine;

public class UnitSelected : State<Battle> {
    private Battler _battler;
    private PathFinder _pathFinder;
    private TileOverlay _overlay;
    private Tile[] _moveRange;
    private Tile _tileUnderMouse;
    private Stack<Tile> _movePath;

    public UnitSelected(Battler battler) {
        _battler = battler;
    }

    public override void Start(Battle battle) {
        base.Start(battle);
	_overlay = GameObject.FindObjectOfType<TileOverlay>();
    }

    public override void Enter(Battle battle) {
        base.Enter(battle);
        _pathFinder = new PathFinder(battle.map, _battler);
	_moveRange = _pathFinder.TilesInRange;
	_overlay.HighlightTiles(_moveRange, TileOverlay.HighlightType.Move);
	_overlay.ShowFocus(_battler.tile);
    }

    public override void Update(Battle battle) {
        base.Update(battle);
        var tile = battle.map.mouse.tileUnderMouse;
        if (tile != null && _tileUnderMouse != tile && _battler.hasMoveAction) { // new tile under mouse
            if (_pathFinder.CostToTile(tile) <= _battler.MoveRange) {
                _movePath = _pathFinder.PathToTile(tile);
            }
            else {
                _movePath = null;
            }
            if (_movePath != null) {
                _overlay.DrawPath(_movePath);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_movePath == null) {
                battle.states.Pop();
            }
            else {
                battle.states.Push(new MoveUnit(_battler, _movePath));
            }
        }
    }

    public override void Exit(Battle obj) {
        base.Exit(obj);
        _movePath = null;
        _overlay.ClearAll();
    }
}
