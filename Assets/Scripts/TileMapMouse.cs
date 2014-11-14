using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapMesh))]
public class TileMapMouse : MonoBehaviour {
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
	    int y = Mathf.FloorToInt(point.z / _tileMap.tileSize);
	    Debug.Log("tile " + x + " , " + y);
        }
    }
}
