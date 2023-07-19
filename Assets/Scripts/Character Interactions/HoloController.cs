using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloController : MonoBehaviour
{
    static HoloController active;
    public Character model;
    private bool placementReady = true;
    // Start is called before the first frame update
    void Start()
    {
        active = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (active != this || Input.GetKey(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
        else
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.Rotate(0f, Input.mouseScrollDelta.y * 10f, 0f);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 location = new Vector3(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));

               transform.SetPositionAndRotation(location, transform.rotation);

                if (placementReady && Input.GetMouseButton(0))
                {
                    Character newModel = Instantiate(model);
                    newModel.transform.SetPositionAndRotation(transform.position, transform.rotation);
                    placementReady = false;
                }
            }
            if (!Input.GetMouseButton(0))
            {
                placementReady = true;
            }
        }
    }
}
