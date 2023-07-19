using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterClicker : MonoBehaviour
{
    [SerializeField] private float cameraHeight = 0f;
    [SerializeField] GameObject highlight;
    private UI userInterface;

    private void Start()
    {
        Renderer frame = GetComponent<Renderer>();
        userInterface = FindObjectOfType<UI>();
        if (cameraHeight == 0)
        {
            cameraHeight = frame.bounds.extents.y * 1.5f;
        }
        else
        {
            cameraHeight *= GameController.mapScale;
        }
    }

    private void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    //imported characters don't have a renderer on their top level. Look at restructuring code to either support placing the Character script at a lower level, or picking up clicks from the level with the renderer.
    private void OnMouseDown()
    {
        if (Cursor.lockState != CursorLockMode.Locked && !EventSystem.current.IsPointerOverGameObject())
        {
            // destroy all existing object highlights and create a new one at the base of the clicked on target.
            foreach (GameObject highlight in GameObject.FindGameObjectsWithTag("highlight"))
            {
                Destroy(highlight);
            }
            GameObject selector = Instantiate(highlight, new Vector3(transform.position.x, GetComponent<Renderer>().bounds.center.y - GetComponent<Renderer>().bounds.size.y / 2 + (.5f*GameController.mapScale), transform.position.z), Quaternion.identity);
            userInterface.CharacterSelected();
            selector.GetComponent<Highlight>().cameraHeight = cameraHeight;
            selector.gameObject.transform.SetParent(transform);
            //would be nice if it instantiated as a child of the character it's on, so destroying the parent doesn't leave a floating highlight until the next click.
        }
    }

    public float GetCameraHeight()
    {
        return cameraHeight;
    }

    public void SetCameraHeight(float height)
    {
        cameraHeight = height;
    }
}
