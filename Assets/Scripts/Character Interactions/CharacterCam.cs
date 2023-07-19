using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCam : MonoBehaviour
{
    Transform cameraCase;
    // Update is called once per frame
        void Update()
    {
        float yAxis = Input.GetAxis("Mouse Y");
        float xAngle = transform.rotation.eulerAngles.x;
        //Debug.Log("axis: " + axis + ", angle: " + transform.rotation.eulerAngles.x);
        if ((xAngle > 70 & xAngle < 90 & yAxis > 0) | (xAngle < 290 & xAngle > 270 & yAxis < 0) | xAngle < 70 | xAngle > 290)
        {
            transform.Rotate(new Vector3(-yAxis * 2.0f, 0.0f, 0.0f));
        }
    }
}
