using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        Debug.Log(rend.material.GetColorArray("_Color"));
        rend.material.SetColor("_BaseColor", Color.blue);
        Debug.Log(rend.material.GetColorArray("_Color"));

    }

}
