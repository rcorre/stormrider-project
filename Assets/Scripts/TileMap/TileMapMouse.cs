using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapMesh))]
[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
    public Transform selectionCube;
    TileMapMesh _mesh;
    TileMap _map;

    public Tile tileUnderMouse { get; private set; }

    void Start() {
        _map = GetComponent<TileMap>();
        _mesh = GetComponent<TileMapMesh>();
    }

    void Update() {
        tileUnderMouse = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            var point = transform.worldToLocalMatrix.MultiplyVector(hitInfo.point);

            int col = Mathf.FloorToInt(point.x / _mesh.tileSize);
            int row = Mathf.FloorToInt(point.z / _mesh.tileSize);

            tileUnderMouse = _map.TileAt(row, col);

            float x = col * _mesh.tileSize;
            float z = row * _mesh.tileSize;
            float y = point.y;

            selectionCube.transform.position = new Vector3(x, y, z);
        }
    }
}
