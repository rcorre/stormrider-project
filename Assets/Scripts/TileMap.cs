using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapGenerator))]
[RequireComponent(typeof(TileMapMesh))]
public class TileMap : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void GenerateMap() {
        var mapGen = GetComponent<TileMapGenerator>();
        var mapMesh = GetComponent<TileMapMesh>();

        var data = mapGen.GenerateMap();
        mapMesh.BuildMesh(data);
    }
}
