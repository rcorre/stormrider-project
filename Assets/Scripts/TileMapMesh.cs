using UnityEngine;
using System.Collections;

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

        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[numTriangles * 3];

        for (int row = 0; row < map.numRows; row++) {
            for (int col = 0; col < map.numCols; col++) {
                var tile = map.tileAt(row, col);
		// get world coordinates that correspond to tile
                float height = map.tileAt(row, col).elevation * heightScale;
                float left   = row * tileSize;
                float right  = row * tileSize + tileSize;
                float top    = col * tileSize;
                float bottom = col * tileSize + tileSize;
		// get the indices of the 4 vertices owned by this tile
                int v0 = col * 2 + row * numCols * 4; // top left
                int v1 = v0 + 1;		    // top right
                int v2 = v0 + numCols * 2;	    // bottom left
                int v3 = v2 + 1;		    // bottom right
		// assign world-space positions to the vertices
                vertices[v0] = new Vector3(left , height, top);
                vertices[v1] = new Vector3(right, height, top);
                vertices[v2] = new Vector3(left , height, bottom);
                vertices[v3] = new Vector3(right, height, bottom);
		// all normals point up for flat surface
                normals[v0] = Vector3.up;
                normals[v1] = Vector3.up;
                normals[v2] = Vector3.up;
                normals[v3] = Vector3.up;
		// TODO: compute uvs based on terrain type, match corresponding point on texture
                float uvX = (float)col / numCols;
                float uvY = (float)row / numCols;
                uv[v0] = new Vector2(uvX, uvY);
                uv[v1] = new Vector2(uvX, uvY);
                uv[v2] = new Vector2(uvX, uvY);
                uv[v3] = new Vector2(uvX, uvY);
		// assign vertex indices to the two triangles owned by this tile
                int tileIdx = row * numCols + col;
                int triIdx = tileIdx * 6;
                triangles[triIdx + 0] = v0;
                triangles[triIdx + 2] = v3;
                triangles[triIdx + 1] = v2;
                triangles[triIdx + 3] = v0;
                triangles[triIdx + 5] = v1;
                triangles[triIdx + 4] = v3;
            }
        }

        // create and populate a new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        // apply mesh to filter/renderer/collider
        ApplyMesh(mesh);
        BuildTexture();
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
