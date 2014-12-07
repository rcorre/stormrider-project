using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapGenerator))]
[RequireComponent(typeof(TileMapMesh))]
public class TileMap : MonoBehaviour {
    private Tile[,] _tiles;

    public int numRows { get { return _tiles.GetLength(0); } }
    public int numCols { get { return _tiles.GetLength(1); } }

    public void GenerateMap() {
        var mapGen = GetComponent<TileMapGenerator>();
        var mapMesh = GetComponent<TileMapMesh>();

        _tiles = mapGen.GenerateMap();
        mapMesh.BuildMesh(this);
    }

    public Tile TileAt(int row, int col) {
        if (row < 0 || row >= numRows || col < 0 || col >= numCols) {
            Debug.LogError(string.Format("tile coords ({0},{1}) are out of bounds ({2},{3})", row, col, numCols, numRows));
        }
        return _tiles[row, col];
    }
}