using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapMesh))]
public class TileMapMouse : MonoBehaviour {
    public Transform selectionCube;
    TileMapMesh _tileMap;

    void Start() {
        _tileMap = GetComponent<TileMapMesh>();
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            var point = transform.worldToLocalMatrix.MultiplyVector(hitInfo.point);
            int x = Mathf.FloorToInt(point.x / _tileMap.tileSize);
            int z = Mathf.FloorToInt(point.z / _tileMap.tileSize);

            selectionCube.transform.position = new Vector3(x * _tileMap.tileSize, 0, z * _tileMap.tileSize);
        }
    }
}
