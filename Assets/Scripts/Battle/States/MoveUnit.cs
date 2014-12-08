using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveUnit : State<Battle> {
    const float closeEnough = 1f;  // say unit reached tile center if this close
    private Battler _battler;
    private Stack<Tile> _path;
    TileMapMesh _mesh;

    public MoveUnit(Battler battler, Stack<Tile> path) {
        _battler = battler;
        _path = path;
    }

    public override void Start(Battle battle) {
        base.Start(battle);
        _mesh = battle.map.mesh;
        _battler.hasMoveAction = false;
    }

    public override void Update(Battle battle) {
        base.Update(battle);
        var targetPos = _mesh.TileSurfaceCenter(_path.Peek()) + Vector3.up * _battler.ObjectHeight / 2;
        var currentPos = _battler.transform.position;
        var disp = targetPos - currentPos;
        var movement = Vector3.ClampMagnitude(disp, _battler.TileMapMoveSpeed * Time.deltaTime);
        _battler.transform.position += movement;
        // check new position and figure out if actor is close enough
        currentPos = _battler.transform.position;
        if (Vector3.Distance(currentPos, targetPos) < closeEnough) { // reached tile
            var tile = _path.Pop();
            if (_path.Count == 0) { // reached destination
                _battler.PlaceOnTile(tile);
                battle.states.Pop();
            }
        }
    }
}
