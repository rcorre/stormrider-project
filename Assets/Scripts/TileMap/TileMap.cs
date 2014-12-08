using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TileMapGenerator))]
[RequireComponent(typeof(TileMapMesh))]
[RequireComponent(typeof(TileMapMouse))]
public class TileMap : MonoBehaviour {
    private Tile[,] _tiles;

    public int numRows { get { return _tiles.GetLength(0); } }
    public int numCols { get { return _tiles.GetLength(1); } }

    public TileMapMouse mouse { get; private set; }
    public TileMapMesh mesh { get; private set; }

    void Awake() {
        mouse = GetComponent<TileMapMouse>();
        mesh = GetComponent<TileMapMesh>();
    }

    public void GenerateMap() {
        var mapGen = GetComponent<TileMapGenerator>();
        var mapMesh = GetComponent<TileMapMesh>();

        _tiles = mapGen.GenerateMap();
        mapMesh.BuildMesh(this);
    }

    /// <summary>
    /// retrieve a tile based on row and column
    /// </summary>
    /// <param name="row">row increases with +z</param>
    /// <param name="col">col increases with +x</param>
    /// <returns>the tile at [row, col], or null if no tile</returns>
    public Tile TileAt(int row, int col) {
        if (row < 0 || row >= numRows || col < 0 || col >= numCols) {
            Debug.LogError(string.Format("tile coords ({0},{1}) are out of bounds ({2},{3})", row, col, numCols, numRows));
            return null;
        }
        return _tiles[row, col];
    }

    public Tile[] TileNeighbors(int row, int col) {
        List<Tile> neighbors = new List<Tile>();
        if (row > 0) { neighbors.Add(_tiles[row - 1, col]); }
        if (col > 0) { neighbors.Add(_tiles[row, col - 1]); }
        if (row < numRows - 1) { neighbors.Add(_tiles[row + 1, col]); }
        if (col < numCols - 1) { neighbors.Add(_tiles[row, col + 1]); }
        return neighbors.ToArray();
    }

    public Tile[] TileNeighbors(Tile tile) {
        return TileNeighbors(tile.row, tile.col);
    }

}