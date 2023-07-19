using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    Camera cam;
    Color[] colors;
    Vector3[] vertices;

    void Start()
    {
        cam = GetComponent<Camera>();
        Debug.Log((float)1 / 2);
    }

    void Update()
    {
        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Mesh mesh = meshCollider.sharedMesh;
        vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawLine(p0, p1);
        Debug.DrawLine(p1, p2);
        Debug.DrawLine(p2, p0);
    }

    public Color[] ColorChange()
    {
        if (colors == null || colors.Length != vertices.Length)
        {
            colors = new Color[vertices.Length];
        }
        for (int c = 0, z = 0; z <= GameController.meshLength; z++)
        {
            for (int x = 0; x <= GameController.meshWidth; x++)
            {
                colors[c] = new Color(1,1,1,255 - (255*((float)(c/(GameController.meshWidth+1))/GameController.meshLength))); //should go from fully opaque to fully transparent by row
            }
        }
        return colors;
    }
}
