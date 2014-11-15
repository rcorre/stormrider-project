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
        int sizeX = map.numCols;
        int sizeZ = map.numRows;
        // prepare data structures
        int numTiles = sizeX * sizeZ;

        int numVerticesX = sizeX + 1;
        int numVerticesZ = sizeZ + 1;
        int numVertices = numVerticesX * numVerticesZ;

        int numTriangles = numTiles * 2;

        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[numTriangles * 3];

        // generate vertices, normals, and uvs
        for (int z = 0; z < numVerticesZ; z++) { // vertex row
            for (int x = 0; x < numVerticesX; x++) { // vertex col
                int row = (z == 0) ? 0 : z - 1;
                int col = (x == 0) ? 0 : x - 1;
                float height = map.tileAt(row, col).elevation * heightScale;
                int idx = z * numVerticesX + x;
                vertices[idx] = new Vector3(x * tileSize, height, z * tileSize);
                normals[idx] = Vector3.up;
                uv[idx] = new Vector2((float)x / (numVerticesX + 1), (float)z / (numVerticesZ + 1));
            }
        }

        // assign triangle vertices
        for (int z = 0; z < sizeZ; z++) { // vertex row
            for (int x = 0; x < sizeX; x++) { // vertex col
                int tileIdx = z * sizeX + x;
                int triIdx = tileIdx * 6;
                int vertOffset = z * numVerticesX + x;

                triangles[triIdx + 0] = vertOffset + 0;
                triangles[triIdx + 2] = vertOffset + numVerticesX + 1;
                triangles[triIdx + 1] = vertOffset + numVerticesX + 0;

                triangles[triIdx + 3] = vertOffset + 0;
                triangles[triIdx + 5] = vertOffset + 1;
                triangles[triIdx + 4] = vertOffset + numVerticesX + 1;
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
