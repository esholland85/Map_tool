using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewButton : MonoBehaviour
{
    Camera cam;
    public void CharacterView()
    {
        cam = Highlight.cam;
        cam.enabled = true;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
