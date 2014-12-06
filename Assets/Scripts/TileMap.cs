using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapGenerator))]
[RequireComponent(typeof(TileMapMesh))]
public class TileMap : MonoBehaviour {
    private TileMapData _data;

    public void setElevation(int row, int col, int elevation) {
    }

    public void GenerateMap() {
        var mapGen = GetComponent<TileMapGenerator>();
        var mapMesh = GetComponent<TileMapMesh>();

        _data = mapGen.GenerateMap();
        mapMesh.BuildMesh(_data);
    }
}