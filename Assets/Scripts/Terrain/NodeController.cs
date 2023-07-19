using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public int index;
    public float height;

    private void Start()
    {
        if (GameController.currentNodes[index] != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GameController.currentNodes[index] = this;
        }
    }

    private void OnMouseDown()
    {
        if (GameController.terrainBrush == "details")
        {
            Destroy(gameObject);
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
