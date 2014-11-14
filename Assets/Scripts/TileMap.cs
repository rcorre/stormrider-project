﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {

    // Use this for initialization
    void Start() {
        BuildMesh();
    }

    // Update is called once per frame
    void Update() {

    }

    void BuildMesh() {
        // prepare data structures
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[2 * 3];
        Vector3[] normals = new Vector3[4];

        // generate mesh data
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(0, 0, -1);
        vertices[3] = new Vector3(1, 0, -1);

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 1;
        triangles[5] = 3;

        normals[0] = Vector3.up;
        normals[1] = Vector3.up;
        normals[2] = Vector3.up;
        normals[3] = Vector3.up;

        // create and populate a new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // apply mesh to filter/renderer/collider
        ApplyMesh(mesh);
    }

    void ApplyMesh(Mesh mesh) {
        var filter = GetComponent<MeshFilter>();
        var renderer = GetComponent<MeshRenderer>();
        var collider = GetComponent<MeshCollider>();

        filter.mesh = mesh;
    }
}
