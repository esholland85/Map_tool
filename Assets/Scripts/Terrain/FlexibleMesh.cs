using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class FlexibleMesh : NetworkBehaviour
{
    public Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;

    public int meshWidth = 20;
    public int meshLength = 20;
    public List<float> heightValues = new List<float>();

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //needs an input for width and length
        if (meshWidth > 0 && meshLength > 0)
        {
            CreateShape();
            UpdateHeights();
        }
        else if (meshWidth > 0 && meshLength == 0)
        {
            WideMesh();
            WideHeights();
        }
        else if (meshWidth == 0 && meshLength > 0)
        {
            LongMesh();
            LongHeights();
        }
        else
        {
            Destroy(gameObject);
        }
        UpdateMesh();
    }

    //need special cases for when length or width == 0
    public void CreateShape()
    {
        vertices = new Vector3[(meshWidth + 1) * (meshLength + 1)];

        for (int i = 0, z = 0; z <= meshLength; z++)
        {
            for (float x = 0; x <= meshWidth; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[meshWidth * meshLength * 6];
        for (int z = 0; z < meshLength; z++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + meshWidth + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + meshWidth + 1;
                triangles[5 + tris] = vert + meshWidth + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= meshLength; z++)
        {
            for (float x = 0; x <= meshWidth; x++)
            {
                uvs[i] = new Vector2((float)x / meshWidth, (float)z / meshLength);
                i++;
            }
        }
    }

    public void WideMesh()
    {
        meshLength = 1;
        vertices = new Vector3[(meshWidth + 1) * (meshLength + 1)];

        for (int i = 0, z = 0; z <= meshLength; z++)
        {
            for (float x = 0; x <= meshWidth; x++)
            {
                vertices[i] = new Vector3(x, 0, z*.25f);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[meshWidth * meshLength * 6];
        for (int z = 0; z < meshLength; z++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + meshWidth + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + meshWidth + 1;
                triangles[5 + tris] = vert + meshWidth + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
        transform.Translate(0, 0, -.125f);
    }

    public void LongMesh()
    {
        meshWidth = 1;
        vertices = new Vector3[(meshWidth + 1) * (meshLength + 1)];

        for (int i = 0, z = 0; z <= meshLength; z++)
        {
            for (float x = 0; x <= meshWidth; x++)
            {
                vertices[i] = new Vector3(x * .25f, 0, z);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[meshWidth * meshLength * 6];
        for (int z = 0; z < meshLength; z++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + meshWidth + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + meshWidth + 1;
                triangles[5 + tris] = vert + meshWidth + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
        transform.Translate(-.125f, 0, 0);
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;

        //updates mesh collider to match mesh renderer
        //MeshCollider meshCollider = GetComponent<MeshCollider>();
        //meshCollider.sharedMesh = mesh;
        //mesh.RecalculateNormals();
    }

    private void UpdateHeights()
    {
        for (int i = 0; i < heightValues.Count; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, heightValues[i], vertices[i].z);
        }
    }

    private void WideHeights()
    {
        Debug.Log(heightValues.Count);
        Debug.Log(vertices.Length);
        for (int i = 0; i < heightValues.Count; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, heightValues[i], vertices[i].z);
            vertices[i + meshWidth + 1] = new Vector3(vertices[i + meshWidth + 1].x, heightValues[i], vertices[i + meshWidth + 1].z);
        }
    }

    private void LongHeights()
    {
        Debug.Log(heightValues.Count);
        Debug.Log(vertices.Length);
        for (int i = 0; i < heightValues.Count; i++)
        {
            vertices[i * 2] = new Vector3(vertices[i * 2].x, heightValues[i], vertices[i * 2].z);
            vertices[i * 2 + 1] = new Vector3(vertices[i * 2 + 1].x, heightValues[i], vertices[i * 2 + 1].z);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }
}
