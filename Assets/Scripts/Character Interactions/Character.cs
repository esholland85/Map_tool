using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float mapScale;
    public string modelName;
    public Sprite buttonImage;
    public Renderer frame;
    public HoloController holoFrame;

    public virtual void Start()
    {
        transform.localScale *= GameController.mapScale;
        if (holoFrame != null)
        {
            GameController.characterCollection.Add(this);
        }
    }
}
