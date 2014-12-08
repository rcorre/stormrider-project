﻿using System.Collections.Generic;
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
        _pathFinder = new PathFinder(battle.map, _battler);
	_moveRange = _pathFinder.TilesInRange;
	_overlay = GameObject.FindObjectOfType<TileOverlay>();
	_overlay.HighlightTiles(_moveRange, TileOverlay.HighlightType.Move);
    }

    public override void Update(Battle battle) {
        base.Update(battle);
        var tile = battle.map.mouse.tileUnderMouse;
        if (tile != null && _tileUnderMouse != tile) { // new tile under mouse
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
    }
}
