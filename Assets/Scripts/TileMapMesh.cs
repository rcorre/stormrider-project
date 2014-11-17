using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// responsible for generating the visual representation of the data provided by
/// a TileMapData object
/// It generates a single mesh to represent the entire map
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMapMesh : MonoBehaviour {
    public float tileSize = 1f;      // vertex spacing per tile
    public float heightScale = 0.5f; // vertex height per unit of tile elevation
    public int tileResolution = 8;   // pixels per side of tile

    public void BuildMesh(TileMapData map) {
        int numCols = map.numCols;
        int numRows = map.numRows;
        // prepare data structures
        int numTiles = numCols * numRows;
        // each tile owns 4 vertices -- one for each corner
        int numVertices = numTiles * 4;
	// each tile contains two triangles
        int numTriangles = numTiles * 2;

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();

        for (int row = 0; row < map.numRows; row++) {
            for (int col = 0; col < map.numCols; col++) {
                var tile = map.tileAt(row, col);
		// get world coordinates that correspond to tile
                float height = map.tileAt(row, col).elevation * heightScale;
                float bottom = row * tileSize;
                float top    = row * tileSize + tileSize;
                float left   = col * tileSize;
                float right  = col * tileSize + tileSize;
		// get the indices of the 4 vertices owned by this tile
                int v0 = vertices.Count;
                int v1 = v0 + 1;
                int v2 = v0 + 2;
                int v3 = v0 + 3;
		// assign world-space positions to the vertices
                vertices.Add(new Vector3(left , height, top));
                vertices.Add(new Vector3(right, height, top));
                vertices.Add(new Vector3(left , height, bottom));
                vertices.Add(new Vector3(right, height, bottom));
		// all normals point up for flat surface
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
		// TODO: compute uvs based on terrain type, match corresponding point on texture
                float uvX = (float)col / numCols;
                float uvY = (float)row / numCols;
                uv.Add(new Vector2(uvX, uvY));
                uv.Add(new Vector2(uvX, uvY));
                uv.Add(new Vector2(uvX, uvY));
                uv.Add(new Vector2(uvX, uvY));
		// assign vertex indices to the two triangles owned by this tile
                triangles.Add(v0);
                triangles.Add(v3);
                triangles.Add(v2);
                triangles.Add(v0);
                triangles.Add(v1);
                triangles.Add(v3);
                if (col < numCols - 1) { // create side mesh to next tile
                    var next = map.tileAt(row, col + 1);
                    AddSide(tile, next, vertices, normals, uv, triangles, uvX, uvY);
                }
            }
        }

        // create and populate a new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();

        // apply mesh to filter/renderer/collider
        ApplyMesh(mesh);
        BuildTexture();
    }

    void AddSide(Tile tile, Tile next, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uv, List<int> triangles, float uvX, float uvY) {
        float height = tile.elevation * heightScale;
        float left   = tile.col * tileSize;
        float right  = tile.col * tileSize + tileSize;
        float bottom = tile.row * tileSize;
        float top    = tile.row * tileSize + tileSize;

        int diff = next.elevation - tile.elevation;
        if (diff != 0) {
            //var norm = diff > 0 ? Vector3.left : Vector3.right;
            var norm = Vector3.left;
            var nextHeight = next.elevation * heightScale;
            int v0 = vertices.Count;
            int v1 = v0 + 1;
            int v2 = v0 + 2;
            int v3 = v0 + 3;

            vertices.Add(new Vector3(right, height, top));
            vertices.Add(new Vector3(right, nextHeight, top));
            vertices.Add(new Vector3(right, height, bottom));
            vertices.Add(new Vector3(right, nextHeight, bottom));

            normals.Add(norm);
            normals.Add(norm);
            normals.Add(norm);
            normals.Add(norm);

                triangles.Add(v0);
                triangles.Add(v3);
                triangles.Add(v2);
                triangles.Add(v0);
                triangles.Add(v1);
                triangles.Add(v3);

            uv.Add(new Vector2(uvX, uvY));
            uv.Add(new Vector2(uvX, uvY));
            uv.Add(new Vector2(uvX, uvY));
            uv.Add(new Vector2(uvX, uvY));
        }
    }

    void BuildTexture() {
        //Texture2D texture = new Texture2D(sizeX * tileResolution, sizeZ * tileResolution);
        int texWidth = 10;
        int texHeight = 10;
        Texture2D texture = new Texture2D(texWidth, texHeight);
        for (int z = 0; z < texHeight; z++) {
            for (int x = 0; x < texWidth; x++) {
                Color c = new Color((float)z / texHeight, 0, (float)x / texWidth);
                texture.SetPixel(x, z, c);
            }
        }

        texture.filterMode = FilterMode.Point; // Use Bilinear for blending, Point for no blending
        texture.Apply();
        var renderer = GetComponent<MeshRenderer>();
        renderer.sharedMaterials[0].mainTexture = texture;
    }

    void ApplyMesh(Mesh mesh) {
        var filter = GetComponent<MeshFilter>();
        var collider = GetComponent<MeshCollider>();

        filter.mesh = mesh;
        collider.sharedMesh = mesh;
    }
}
