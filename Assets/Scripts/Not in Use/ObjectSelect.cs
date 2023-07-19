using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelect : MonoBehaviour
{
    [SerializeField] GameObject highlight;

    // SelectObject is called before the first frame update
    void SelectObject()
    {
        if (Input.GetAxis("Fire1")>0)
        {
            Debug.Log("FIRE ZE MISSILES!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            Debug.Log("FIRE ZE MISSILES!");
        }
    }

    private void OnMouseDown()
    {
        // destroy all existing object highlights and create a new one at the base of the clicked on target.
        foreach (GameObject highlight in GameObject.FindGameObjectsWithTag("highlight"))
        {
            Destroy(highlight);
        }
        Instantiate(highlight, new Vector3(transform.position.x, transform.position.y - GetComponent<Renderer>().bounds.size.y/2 + .5f,transform.position.z), Quaternion.identity);
    }
}
