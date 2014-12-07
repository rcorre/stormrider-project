using UnityEngine;
using System.Collections;

/// <summary>
/// wrapper providing controlled access to the data backing a TileMap
/// </summary>
public class TileMapData {
    public int numRows { get { return _tiles.GetLength(0); } }
    public int numCols { get { return _tiles.GetLength(1); } }

    public TileMapData(Tile[,] tiles) {
        _tiles = tiles;
    }

    public Tile tileAt(int row, int col) {
        if (row < 0 || row >= numRows || col < 0 || col >= numCols) {
            Debug.LogError(string.Format("tile coords ({0},{1}) are out of bounds ({2},{3})", row, col, numCols, numRows));
        }
        return _tiles[row, col];
    }

    private Tile[,] _tiles;
}
