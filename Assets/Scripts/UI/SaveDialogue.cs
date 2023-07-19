using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveDialogue : MonoBehaviour
{
    float windowWidth = 200f;
    Rect windowRect;
    public Texture2D image;
    string userInput;

    private void Awake()
    {
        userInput = GameController.uiName;
    }

    void OnGUI()
    {
        windowRect = new Rect((Screen.width-windowWidth)/2, 100, windowWidth, 50);
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Save Name?");
    }

    void DoMyWindow(int windowID)
    {
        GUI.skin.window.normal.background = image;
        GUI.skin.window.onNormal.background = image;

        GUILayout.Space(20);

        userInput = GUILayout.TextField(userInput, GUILayout.MaxWidth(windowWidth-20));

        GUILayout.Space(20);

        if (GUILayout.Button("Save"))
        {
            GameController.uiName = userInput;
            SaveSystem.SaveMap();
            gameObject.SetActive(false);
        }
        if (GUILayout.Button("Return"))
        {
            Debug.Log(GameController.uiName);
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
