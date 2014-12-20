using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// responsible for generating the visual representation of the data provided by
/// a TileMapData object
/// It generates a single mesh to represent the entire map
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(TileMapTexture))]
public class TileMapMesh : MonoBehaviour {
    public float tileSize = 1f;      // vertex spacing per tile
    public float heightScale = 0.5f; // vertex height per unit of tile elevation
    public int tileResolution = 8;   // pixels per side of tile

    public void BuildMesh(TileMap map) {
        var meshData = new MeshData();
        var texturizer = GetComponent<TileMapTexture>();

        for (int row = 0; row < map.numRows; row++) {
            for (int col = 0; col < map.numCols; col++) {
                var tile = map.TileAt(row, col);
                // get world coordinates that correspond to tile
                float height = map.TileAt(row, col).elevation * heightScale;
                float bottom = row * tileSize;
                float top = row * tileSize + tileSize;
                float left = col * tileSize;
                float right = col * tileSize + tileSize;
                // assign these positions to the 4 vertices owned by this tile
                var vertices = new Vector3[] {
		    new Vector3(left, height, top),
		    new Vector3(right, height, top),
                    new Vector3(left, height, bottom),
                    new Vector3(right, height, bottom)
		};

                // get uv coords based on elevation
                var uvs = texturizer.elevationToUv(tile.elevation);
                var normal = Vector3.up;
                // add top surface of tile to mesh
                AddSurface(meshData, vertices, uvs, normal);
                // add right and bottom surfaces of tile to mesh
                if (col < map.numCols - 1) { // create mesh to next tile in x direction
                    var tileToRight = map.TileAt(row, col + 1);
                    AddSideX(tile, tileToRight, meshData, uvs);
                }
                if (row < map.numRows - 1) { // create mesh to next tile in z direction
                    var tileAbove = map.TileAt(row + 1, col);
                    AddSideZ(tile, tileAbove, meshData, uvs);
                }
            }
        }

        // apply mesh to filter/renderer/collider
        ApplyMesh(meshData);
    }

    /// <summary>
    /// check if a ray intersects a tile in the mesh
    /// </summary>
    /// <param name="ray">ray to check for intersection</param>
    /// <param name="row">row that ray intersected</param>
    /// <param name="col">col that ray intersected</param>
    /// <returns>true if the ray intersected a tile</returns>
    public bool RayCastToTile(Ray ray, out int row, out int col) {
        RaycastHit hitInfo;
        if (collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            var point = transform.worldToLocalMatrix.MultiplyVector(hitInfo.point);
            row = Mathf.FloorToInt(point.x / tileSize);
            col = Mathf.FloorToInt(point.z / tileSize);
            return true;
        }
        row = col = 0;
        return false;
    }

    public Vector3 TileSurfaceCenter(Tile tile) {
        float x = tile.col * tileSize + tileSize / 2;
        float y = tile.elevation * heightScale;
        float z = tile.row * tileSize + tileSize / 2;
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// add a side between tile and next (the tile in the following column)
    /// </summary>
    void AddSideX(Tile tile, Tile next, MeshData meshData, Vector2[] uvs) {
        float height = tile.elevation * heightScale;
        float right = tile.col * tileSize + tileSize;
        float bottom = tile.row * tileSize;
        float top = tile.row * tileSize + tileSize;

        var norm = next.elevation > tile.elevation ? Vector3.left : Vector3.right;
        var nextHeight = next.elevation * heightScale;

        var vertices = new Vector3[] {
	    new Vector3(right, height, top),
	    new Vector3(right, nextHeight, top),
	    new Vector3(right, height, bottom),
	    new Vector3(right, nextHeight, bottom)
	};

        AddSurface(meshData, vertices, uvs, norm);
    }

    /// <summary>
    /// add a side between tile and next (the tile in the following row)
    /// </summary>
    void AddSideZ(Tile tile, Tile next, MeshData meshData, Vector2[] uvs) {
        float height = tile.elevation * heightScale;
        float left = tile.col * tileSize;
        float right = tile.col * tileSize + tileSize;
        float top = tile.row * tileSize + tileSize;

        var norm = next.elevation > tile.elevation ? Vector3.back : Vector3.forward;
        var nextHeight = next.elevation * heightScale;

        var vertices = new Vector3[] {
	    new Vector3(left, nextHeight, top),
    	    new Vector3(right, nextHeight, top),
	    new Vector3(left, height, top),
	    new Vector3(right, height, top)
	};

        AddSurface(meshData, vertices, uvs, norm);
    }

    void AddSurface(MeshData meshData, Vector3[] vertices, Vector2[] uvs, Vector3 normal) {
        int v0 = meshData.vertices.Count;
        int v1 = v0 + 1;
        int v2 = v0 + 2;
        int v3 = v0 + 3;

        meshData.vertices.AddRange(vertices);

        meshData.normals.AddRange(new Vector3[] { normal, normal, normal, normal });

        meshData.triangles.AddRange(new int[] { v0, v3, v2 }); // first tri
        meshData.triangles.AddRange(new int[] { v0, v1, v3 }); // second tri

        meshData.uv.AddRange(uvs);
    }

    void ApplyMesh(MeshData meshData) {
        // create and populate a new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.vertices.ToArray();
        mesh.triangles = meshData.triangles.ToArray();
        mesh.normals = meshData.normals.ToArray();
        mesh.uv = meshData.uv.ToArray();
        mesh.Optimize(); // this could increase performance but also incurs higher generation time

        var filter = GetComponent<MeshFilter>();
        var collider = GetComponent<MeshCollider>();

        filter.mesh = mesh;
        collider.sharedMesh = mesh;
    }

    /// <summary>
    /// convenience container for storing related mesh data during generation
    /// </summary>
    private class MeshData {
        public MeshData() {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            uv = new List<Vector2>();
            triangles = new List<int>();
        }
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector2> uv;
        public List<int> triangles;
    }
}
