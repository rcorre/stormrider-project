using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMapMesh : MonoBehaviour {
    public int sizeX = 100; // number of tiles horizontally
    public int sizeZ = 50;  // number of tiles vertically
    public float tileSize = 1f;

    // Use this for initialization
    void Start() {
        BuildMesh();
    }

    // Update is called once per frame
    void Update() {

    }

    public void BuildMesh() {
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
                int idx = z * numVerticesX + x;
                float height = Random.Range(0f, 1f);
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
    }

    void ApplyMesh(Mesh mesh) {
        var filter = GetComponent<MeshFilter>();
        var renderer = GetComponent<MeshRenderer>();
        var collider = GetComponent<MeshCollider>();

        filter.mesh = mesh;
        collider.sharedMesh = mesh;
    }
}
