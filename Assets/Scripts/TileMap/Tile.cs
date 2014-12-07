using UnityEngine;
using System.Collections;

/// <summary>
/// contains the data backing a tile within a tilemap
/// could store information like elevation, terrain/move-cost, ect.
/// </summary>
public class Tile {
    public readonly int row, col, elevation;

    public Tile(int row, int col, int elevation) {
        this.row = row;
        this.col = col;
        this.elevation = elevation;
    }
}
