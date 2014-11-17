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

	var meshData = new MeshData();

        for (int row = 0; row < map.numRows; row++) {
            for (int col = 0; col < map.numCols; col++) {
                var tile = map.tileAt(row, col);
                // get world coordinates that correspond to tile
                float height = map.tileAt(row, col).elevation * heightScale;
                float bottom = row * tileSize;
                float top = row * tileSize + tileSize;
                float left = col * tileSize;
                float right = col * tileSize + tileSize;
                // get the indices of the 4 vertices owned by this tile
                int v0 = meshData.vertices.Count;
                int v1 = v0 + 1;
                int v2 = v0 + 2;
                int v3 = v0 + 3;
                // assign world-space positions to the vertices
                meshData.vertices.Add(new Vector3(left, height, top));
                meshData.vertices.Add(new Vector3(right, height, top));
                meshData.vertices.Add(new Vector3(left, height, bottom));
                meshData.vertices.Add(new Vector3(right, height, bottom));
                // all normals point up for flat surface
                meshData.normals.Add(Vector3.up);
                meshData.normals.Add(Vector3.up);
                meshData.normals.Add(Vector3.up);
                meshData.normals.Add(Vector3.up);
                // TODO: compute uvs based on terrain type, match corresponding point on texture
                float uvX = (float)col / numCols;
                float uvY = (float)row / numCols;
                meshData.uv.Add(new Vector2(uvX, uvY));
                meshData.uv.Add(new Vector2(uvX, uvY));
                meshData.uv.Add(new Vector2(uvX, uvY));
                meshData.uv.Add(new Vector2(uvX, uvY));
                // assign vertex indices to the two triangles owned by this tile
                meshData.triangles.Add(v0);
                meshData.triangles.Add(v3);
                meshData.triangles.Add(v2);
                meshData.triangles.Add(v0);
                meshData.triangles.Add(v1);
                meshData.triangles.Add(v3);
                if (col < numCols - 1) { // create side mesh to next tile
                    var tileToRight = map.tileAt(row, col + 1);
                    AddSideX(tile, tileToRight, meshData, uvX, uvY);
                }

                if (row < numRows - 1) { // create side mesh to next tile
                    var tileAbove = map.tileAt(row + 1, col);
                    AddSideZ(tile, tileAbove, meshData, uvX, uvY);
                }
            }
        }

        // apply mesh to filter/renderer/collider
        ApplyMesh(meshData);
        BuildTexture();
    }

    void AddSideX(Tile tile, Tile next, MeshData meshData, float uvX, float uvY) {
        float height = tile.elevation * heightScale;
        float left = tile.col * tileSize;
        float right = tile.col * tileSize + tileSize;
        float bottom = tile.row * tileSize;
        float top = tile.row * tileSize + tileSize;

        int diff = next.elevation - tile.elevation;
        if (diff != 0) {
            var norm = diff > 0 ? Vector3.left : Vector3.right;
            var nextHeight = next.elevation * heightScale;
            int v0 = meshData.vertices.Count;
            int v1 = v0 + 1;
            int v2 = v0 + 2;
            int v3 = v0 + 3;

            meshData.vertices.Add(new Vector3(right, height, top));
            meshData.vertices.Add(new Vector3(right, nextHeight, top));
            meshData.vertices.Add(new Vector3(right, height, bottom));
            meshData.vertices.Add(new Vector3(right, nextHeight, bottom));

            meshData.normals.Add(norm);
            meshData.normals.Add(norm);
            meshData.normals.Add(norm);
            meshData.normals.Add(norm);

            meshData.triangles.Add(v0);
            meshData.triangles.Add(v3);
            meshData.triangles.Add(v2);
            meshData.triangles.Add(v0);
            meshData.triangles.Add(v1);
            meshData.triangles.Add(v3);

            meshData.uv.Add(new Vector2(uvX, uvY));
            meshData.uv.Add(new Vector2(uvX, uvY));
            meshData.uv.Add(new Vector2(uvX, uvY));
            meshData.uv.Add(new Vector2(uvX, uvY));
        }
    }

    void AddSideZ(Tile tile, Tile next, MeshData meshData, float uvX, float uvY) {
        float height = tile.elevation * heightScale;
        float left   = tile.col * tileSize;
        float right  = tile.col * tileSize + tileSize;
        float bottom = tile.row * tileSize;
        float top    = tile.row * tileSize + tileSize;

        int diff = next.elevation - tile.elevation;
        if (diff != 0) {
            var norm = diff > 0 ? Vector3.back : Vector3.forward;
            var nextHeight = next.elevation * heightScale;
            int v0 = meshData.vertices.Count;
            int v1 = v0 + 1;
            int v2 = v0 + 2;
            int v3 = v0 + 3;

            meshData.vertices.Add(new Vector3(left, nextHeight, top));
            meshData.vertices.Add(new Vector3(right, nextHeight, top));
            meshData.vertices.Add(new Vector3(left, height, top));
            meshData.vertices.Add(new Vector3(right, height, top));

            meshData.normals.Add(norm);
            meshData.normals.Add(norm);
            meshData.normals.Add(norm);
            meshData.normals.Add(norm);

            meshData.triangles.Add(v0);
            meshData.triangles.Add(v3);
            meshData.triangles.Add(v2);
            meshData.triangles.Add(v0);
            meshData.triangles.Add(v1);
            meshData.triangles.Add(v3);

            meshData.uv.Add(new Vector2(uvX, uvY));
            meshData.uv.Add(new Vector2(uvX, uvY));
            meshData.uv.Add(new Vector2(uvX, uvY));
            meshData.uv.Add(new Vector2(uvX, uvY));
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

    void ApplyMesh(MeshData meshData) {
        // create and populate a new mesh
        Mesh mesh = new Mesh();
        mesh.vertices  = meshData.vertices.ToArray();
        mesh.triangles = meshData.triangles.ToArray();
        mesh.normals   = meshData.normals.ToArray();
        mesh.uv        = meshData.uv.ToArray();
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
