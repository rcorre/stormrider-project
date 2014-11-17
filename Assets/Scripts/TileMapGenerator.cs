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
        int[,] heightMap = new int[numRows, numCols];
        CreateHill(heightMap, 5, 5, 3); // TEST

        var tiles = new Tile[numRows, numCols];
        for (int row = 0; row < numRows; row++) {
            for (int col = 0; col < numCols; col++) {
                int elevation = heightMap[row, col];
                tiles[row, col] = new Tile(row, col, elevation);
            }
        }

        return new TileMapData(tiles);
    }

    void CreateHill(int[,] heightMap, int centerRow, int centerCol, int elevation) {
        int startRow = Mathf.Clamp(centerRow - elevation, 0, numRows);
        int endRow   = Mathf.Clamp(centerRow + elevation, 0, numRows);
        int startCol = Mathf.Clamp(centerCol - elevation, 0, numCols);
        int endCol   = Mathf.Clamp(centerCol + elevation, 0, numCols);

        for (int row = startRow; row < endRow; row++) {
            for (int col = startCol; col < endCol; col++) {
                // distance from center of hill
                int dist = Mathf.Abs(row - centerRow) + Mathf.Abs(col - centerCol);
                // how much to raise terrain, based on distance from center
                int delta = Mathf.Max(elevation - dist, 0);
                heightMap[row, col] += delta;
            }
        }
    }
}
