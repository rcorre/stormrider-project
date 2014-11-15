using UnityEngine;
using System.Collections;

/// <summary>
/// contains the data backing a tile within a tilemap
/// could store information like elevation, terrain/move-cost, ect.
/// </summary>
public class Tile {
    readonly int elevation;

    public Tile(int elevation) {
        this.elevation = elevation;
    }
}
