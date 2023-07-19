using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public float cameraHeight;
    public static Camera cam;
    public Transform camContainer;

    private void Start()
    {
        // gets the child (contains a camera component) and places it at head-height.
        transform.localScale *= GameController.mapScale;
        cam = camContainer.GetComponent<Camera>();
        cam.nearClipPlane *= GameController.mapScale;
        cam.enabled = false;
        camContainer.transform.position = new Vector3(transform.position.x, transform.position.y + cameraHeight, transform.position.z);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X") * 2.0f, 0.0f));

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Destroy(gameObject);
        }
    }
}
