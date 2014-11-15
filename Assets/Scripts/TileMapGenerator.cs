using UnityEngine;
using System.Collections;

/// <summary>
/// eventually, this class should be in charge of procedural map generation.
/// it will most likely enlist the help of other components for specific
/// tasks like constructing buildings, hills, deserts, ect.
/// </summary>
public class TileMapGenerator : MonoBehaviour {
    public int numRows, numCols;

    public TileMapData GenerateMap() {
        var tiles = new Tile[numRows, numCols];
        for (int row = 0; row < numRows; row++) {
            for (int col = 0; col < numRows; col++) {
                tiles[row, col] = new Tile(0);
            }
        }

        return new TileMapData(tiles);
    }
}
