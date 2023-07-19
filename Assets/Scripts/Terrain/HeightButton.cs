using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeightButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TerrainGenerator mesh;
    public string currentHeight;

    private void OnEnable()
    {
        input.text = GameController.activeNode.height.ToString();
    }

    //used by button to set height
    public void SetHeight()
    {
        /* //may be useful for detail brush, don't delete
        List<NodeHeight> currentNodes = new List<NodeHeight>();
        foreach (NodeHeight node in GameController.currentNodes)
        {
            if (node != null)
            {
                currentNodes.Add(node);
            }

        }
        foreach (NodeHeight node in currentNodes)
        {
            GameController.activeNode = node;
            GameController.activeNode.height = float.Parse(input.text);
            mesh.ChangeHeight();
        }
        */
        int bottomLeft = -1;
        int upperRight = -1;
        foreach (NodeController node in GameController.currentNodes)
        {
            if (node != null && bottomLeft == -1)
            {
                bottomLeft = node.index;
            }
            if (node != null)
            {
                upperRight = node.index;
            }
        }
        mesh.SetHeight(bottomLeft, upperRight, float.Parse(input.text));
    }

    public void AddHeight()
    {
        int bottomLeft = -1;
        int upperRight = -1;
        foreach (NodeController node in GameController.currentNodes)
        {
            if (node != null && bottomLeft == -1)
            {
                bottomLeft = node.index;
            }
            if (node != null)
            {
                upperRight = node.index;
            }
        }
        mesh.AddHeight(bottomLeft, upperRight, float.Parse(input.text));
    }

    public void ToggleTrans()
    {
        int bottomLeft = -1;
        int upperRight = -1;
        foreach (NodeController node in GameController.currentNodes)
        {
            if (node != null && bottomLeft == -1)
            {
                bottomLeft = node.index;
            }
            if (node != null)
            {
                upperRight = node.index;
            }
        }
        mesh.ToggleTransparent(bottomLeft, upperRight);
    }
}
