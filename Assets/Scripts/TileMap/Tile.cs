using UnityEngine;
using System.Collections;

/// <summary>
/// contains the data backing a tile within a tilemap
/// could store information like elevation, terrain/move-cost, ect.
/// </summary>
public class Tile {
    public readonly int row, col, elevation;

    private Battler _battler;
    public Battler battler {
        get { return _battler; }
        set {
            if (_battler != null && value != null) {
                Debug.LogError(string.Format("tried to place battler on tile ({0}, {1}) already containing battler", row, col));
            }
            _battler = value;
        }
    }

    public bool empty { get { return _battler == null; } }

    public Tile(int row, int col, int elevation) {
        this.row = row;
        this.col = col;
        this.elevation = elevation;
    }
}
